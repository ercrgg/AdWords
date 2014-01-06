// Copyright 2013, Google Inc. All Rights Reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

// Author: api.anash@gmail.com (Anash P. Oommen)

using Google.Api.Ads.AdWords.Lib;
using Google.Api.Ads.AdWords.v201309;

using System;
using System.Collections.Generic;
using System.IO;

namespace Google.Api.Ads.AdWords.Examples.CSharp.v201309 {
  /// <summary>
  /// This code example adds a Shared Bidding Strategy and uses it to construct
  /// a campaign.
  ///
  /// Tags: BiddingStrategyService.mutate
  /// Tags: BudgetService.mutate, CampaignService.mutate
  /// </summary>
  public class UseSharedBiddingStrategy : ExampleBase {
    /// <summary>
    /// Main method, to run this code example as a standalone application.
    /// </summary>
    /// <param name="args">The command line arguments.</param>
    public static void Main(string[] args) {
      UseSharedBiddingStrategy codeExample = new UseSharedBiddingStrategy();
      Console.WriteLine(codeExample.Description);
      try {
        codeExample.Run(new AdWordsUser());
      } catch (Exception ex) {
        Console.WriteLine("An exception occurred while running this code example. {0}",
            ExampleUtilities.FormatException(ex));
      }
    }

    /// <summary>
    /// Returns a description about the code example.
    /// </summary>
    public override string Description {
      get {
        return "This code example adds a Shared Bidding Strategy and uses it to construct " +
            "a campaign.";
      }
    }

    /// <summary>
    /// Runs the specified code example.
    /// </summary>
    /// <param name="user">The AdWords user.</param>
    public void Run(AdWordsUser user) {
      // Get the services.
      BiddingStrategyService biddingStrategyService = (BiddingStrategyService) user.GetService(
          AdWordsService.v201309.BiddingStrategyService);

      BudgetService budgetService = (BudgetService) user.GetService(
          AdWordsService.v201309.BudgetService);

      CampaignService campaignService = (CampaignService) user.GetService(
          AdWordsService.v201309.CampaignService);

      String BIDDINGSTRATEGY_NAME = "Maximize Clicks " + ExampleUtilities.GetRandomString();
      const long BID_CEILING = 2000000;
      const long SPEND_TARGET = 20000000;

      String BUDGET_NAME = "Shared Interplanetary Budget #" + ExampleUtilities.GetRandomString();
      const long BUDGET_AMOUNT = 30000000;

      String CAMPAIGN_NAME = "Interplanetary Cruise #" + ExampleUtilities.GetRandomString();

      try {
        SharedBiddingStrategy sharedBiddingStrategy = CreateBiddingStrategy(biddingStrategyService,
            BIDDINGSTRATEGY_NAME, BID_CEILING, SPEND_TARGET);
          Console.WriteLine("Shared bidding strategy with name '{0}' and ID {1} of type {2} was " +
              "created.", sharedBiddingStrategy.name, sharedBiddingStrategy.id,
              sharedBiddingStrategy.biddingScheme.BiddingSchemeType);

          Budget sharedBudget = CreateSharedBudget(budgetService, BUDGET_NAME, BUDGET_AMOUNT);

          Campaign newCampaign = CreateCampaignWithBiddingStrategy(campaignService, CAMPAIGN_NAME,
              sharedBiddingStrategy.id, sharedBudget.budgetId);

          Console.WriteLine("Campaign with name '{0}', ID {1} and bidding scheme ID {2} was " +
              "created.", newCampaign.name, newCampaign.id,
              newCampaign.biddingStrategyConfiguration.biddingStrategyId);

      } catch (Exception ex) {
        throw new System.ApplicationException("Failed to create campaign that uses shared " +
            "bidding strategy.", ex);
      }
    }

    /// <summary>
    /// Creates the shared bidding strategy.
    /// </summary>
    /// <param name="biddingStrategyService">The bidding strategy service.</param>
    /// <param name="name">The bidding strategy name.</param>
    /// <param name="bidCeiling">The bid ceiling.</param>
    /// <param name="spendTarget">The spend target.</param>
    /// <returns>The bidding strategy object.</returns>
    private SharedBiddingStrategy CreateBiddingStrategy(
        BiddingStrategyService biddingStrategyService, String name, long bidCeiling,
        long spendTarget) {
      // Create a shared bidding strategy.
      SharedBiddingStrategy sharedBiddingStrategy = new SharedBiddingStrategy();
      sharedBiddingStrategy.name = name;

      TargetSpendBiddingScheme biddingScheme = new TargetSpendBiddingScheme();
      // Optionally set additional bidding scheme parameters.
      biddingScheme.bidCeiling = new Money();
      biddingScheme.bidCeiling.microAmount = bidCeiling;

      biddingScheme.spendTarget = new Money();
      biddingScheme.spendTarget.microAmount = spendTarget;

      sharedBiddingStrategy.biddingScheme = biddingScheme;

      // Create operation.
      BiddingStrategyOperation operation = new BiddingStrategyOperation();
      operation.@operator = Operator.ADD;
      operation.operand = sharedBiddingStrategy;

      return biddingStrategyService.mutate(new BiddingStrategyOperation[] {operation}).value[0];
    }

    /// <summary>
    /// Creates an explicit budget to be used only to create the Campaign.
    /// </summary>
    /// <param name="budgetService">The budget service.</param>
    /// <param name="name">The budget name.</param>
    /// <param name="amount">The budget amount.</param>
    /// <returns>The budget object.</returns>
    private Budget CreateSharedBudget(BudgetService budgetService, String name, long amount) {
      // Create a shared budget
      Budget budget = new Budget();
      budget.name = name;
      budget.period = BudgetBudgetPeriod.DAILY;
      budget.amount = new Money();
      budget.amount.microAmount = amount;
      budget.deliveryMethod = BudgetBudgetDeliveryMethod.STANDARD;
      budget.isExplicitlyShared = true;

      // Create operation.
      BudgetOperation operation = new BudgetOperation();
      operation.operand = budget;
      operation.@operator = Operator.ADD;

      // Make the mutate request.
      return budgetService.mutate(new BudgetOperation[] {operation}).value[0];
    }

    /// <summary>
    /// Creates the campaign with a shared bidding strategy.
    /// </summary>
    /// <param name="campaignService">The campaign service.</param>
    /// <param name="name">The campaign name.</param>
    /// <param name="biddingStrategyId">The bidding strategy id.</param>
    /// <param name="sharedBudgetId">The shared budget id.</param>
    /// <returns>The campaign object.</returns>
    private Campaign CreateCampaignWithBiddingStrategy(CampaignService campaignService, string name,
        long biddingStrategyId, long sharedBudgetId) {
      // Create campaign.
      Campaign campaign = new Campaign();
      campaign.name = name;

      // Set the budget.
      campaign.budget = new Budget();
      campaign.budget.budgetId = sharedBudgetId;

      // Set bidding strategy (required).
      BiddingStrategyConfiguration biddingStrategyConfiguration =
          new BiddingStrategyConfiguration();
      biddingStrategyConfiguration.biddingStrategyId = biddingStrategyId;

      campaign.biddingStrategyConfiguration = biddingStrategyConfiguration;

      // Set keyword matching setting (required).
      KeywordMatchSetting keywordMatchSetting = new KeywordMatchSetting();
      keywordMatchSetting.optIn = true;
      campaign.settings = new Setting[] {keywordMatchSetting};

      // Set network targeting (recommended).
      NetworkSetting networkSetting = new NetworkSetting();
      networkSetting.targetGoogleSearch = true;
      networkSetting.targetSearchNetwork = true;
      networkSetting.targetContentNetwork = true;
      campaign.networkSetting = networkSetting;

      // Create operation.
      CampaignOperation operation = new CampaignOperation();
      operation.operand = campaign;
      operation.@operator = Operator.ADD;

      return campaignService.mutate(new CampaignOperation[] {operation}).value[0];
    }
  }
}

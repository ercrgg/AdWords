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
  /// This code example adds various types of targeting criteria to a campaign.
  /// To get a list of campaigns, run GetCampaigns.cs.
  ///
  /// Tags: CampaignCriterionService.mutate
  /// </summary>
  public class AddCampaignTargetingCriteria : ExampleBase {
    /// <summary>
    /// Main method, to run this code example as a standalone application.
    /// </summary>
    /// <param name="args">The command line arguments.</param>
    public static void Main(string[] args) {
      AddCampaignTargetingCriteria codeExample = new AddCampaignTargetingCriteria();
      Console.WriteLine(codeExample.Description);
      try {
        long campaignId = long.Parse("INSERT_CAMPAIGN_ID_HERE");
        codeExample.Run(new AdWordsUser(), campaignId);
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
        return "This code example adds various types of targeting criteria to a campaign. To " +
            "get a list of campaigns, run GetCampaigns.cs.";
      }
    }

    /// <summary>
    /// Runs the code example.
    /// </summary>
    /// <param name="user">The AdWords user.</param>
    /// <param name="campaignId">Id of the campaign to which targeting criteria
    /// are added.</param>
    public void Run(AdWordsUser user, long campaignId) {
      // Get the CampaignCriterionService.
      CampaignCriterionService campaignCriterionService =
          (CampaignCriterionService) user.GetService(
              AdWordsService.v201309.CampaignCriterionService);

      // Create language criteria.
      // See http://code.google.com/apis/adwords/docs/appendix/languagecodes.html
      // for a detailed list of language codes.
      Language language1 = new Language();
      language1.id = 1002; // French
      CampaignCriterion languageCriterion1 = new CampaignCriterion();
      languageCriterion1.campaignId = campaignId;
      languageCriterion1.criterion = language1;

      Language language2 = new Language();
      language2.id = 1005; // Japanese
      CampaignCriterion languageCriterion2 = new CampaignCriterion();
      languageCriterion2.campaignId = campaignId;
      languageCriterion2.criterion = language2;

      // Create location criteria.
      // See http://code.google.com/apis/adwords/docs/appendix/countrycodes.html
      // for a detailed list of country codes.
      Location location1 = new Location();
      location1.id = 2840; // USA
      CampaignCriterion locationCriterion1 = new CampaignCriterion();
      locationCriterion1.campaignId = campaignId;
      locationCriterion1.criterion = location1;

      Location location2 = new Location();
      location2.id = 2392; // Japan
      CampaignCriterion locationCriterion2 = new CampaignCriterion();
      locationCriterion2.campaignId = campaignId;
      locationCriterion2.criterion = location2;

      // Add a negative campaign keyword.
      NegativeCampaignCriterion negativeCriterion = new NegativeCampaignCriterion();
      negativeCriterion.campaignId = campaignId;

      Keyword keyword = new Keyword();
      keyword.matchType = KeywordMatchType.BROAD;
      keyword.text = "jupiter cruise";

      negativeCriterion.criterion = keyword;

      CampaignCriterion[] criteria = new CampaignCriterion[] {languageCriterion1,
          languageCriterion2, locationCriterion1, locationCriterion2, negativeCriterion};

      List<CampaignCriterionOperation> operations = new List<CampaignCriterionOperation>();

      foreach (CampaignCriterion criterion in criteria) {
        CampaignCriterionOperation operation = new CampaignCriterionOperation();
        operation.@operator = Operator.ADD;
        operation.operand = criterion;
        operations.Add(operation);
      }

      try {
        // Set the campaign targets.
        CampaignCriterionReturnValue retVal = campaignCriterionService.mutate(operations.ToArray());

        if (retVal != null && retVal.value != null) {
          // Display campaign targets.
          foreach (CampaignCriterion criterion in retVal.value) {
            Console.WriteLine("Campaign criteria of type '{0}' was set to campaign with" +
                " id = '{1}'.", criterion.criterion.CriterionType, criterion.campaignId);
          }
        }
      } catch (Exception ex) {
        throw new System.ApplicationException("Failed to set Campaign criteria.", ex);
      }
    }
  }
}

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
using Google.Api.Ads.AdWords.v201306;

using System;
using System.Collections.Generic;
using System.IO;

namespace Google.Api.Ads.AdWords.Examples.CSharp.v201306 {
  /// <summary>
  /// This code example deletes a campaign by setting the status to 'DELETED'.
  /// To get campaigns, run GetCampaigns.cs.
  ///
  /// Tags: CampaignService.mutate
  /// </summary>
  public class DeleteCampaign : ExampleBase {
    /// <summary>
    /// Main method, to run this code example as a standalone application.
    /// </summary>
    /// <param name="args">The command line arguments.</param>
    public static void Main(string[] args) {
      DeleteCampaign codeExample = new DeleteCampaign();
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
        return "This code example deletes a campaign by setting the status to 'DELETED'. " +
            "To get campaigns, run GetCampaigns.cs.";
      }
    }

    /// <summary>
    /// Runs the code example.
    /// </summary>
    /// <param name="user">The AdWords user.</param>
    /// <param name="campaignId">Id of the campaign to be deleted.</param>
    public void Run(AdWordsUser user, long campaignId) {
      // Get the CampaignService.
      CampaignService campaignService = (CampaignService) user.GetService(
          AdWordsService.v201306.CampaignService);

      // Create campaign with DELETED status.
      Campaign campaign = new Campaign();
      campaign.id = campaignId;

      // When deleting a campaign, rename it to avoid name collisions with new
      // campaigns.
      campaign.name = "Deleted Campaign - " + ExampleUtilities.GetRandomString();
      campaign.status = CampaignStatus.DELETED;

      // Create the operation.
      CampaignOperation operation = new CampaignOperation();
      operation.operand = campaign;
      operation.@operator = Operator.SET;

      try {
        // Delete the campaign.
        CampaignReturnValue retVal = campaignService.mutate(new CampaignOperation[] {operation});

        // Display the results.
        if (retVal != null && retVal.value != null && retVal.value.Length > 0) {
          Campaign deletedCampaign = retVal.value[0];
          Console.WriteLine("Campaign with id = \"{0}\" was renamed to \"{1}\" and deleted.",
              deletedCampaign.id, deletedCampaign.name);
        } else {
          Console.WriteLine("No campaigns were deleted.");
        }
      } catch (Exception ex) {
        throw new System.ApplicationException("Failed to delete campaign.", ex);
      }
    }
  }
}

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
  /// This code example deletes a placement using the 'REMOVE' operator. To get
  /// placements, run GetPlacements.cs.
  ///
  /// Tags: AdGroupCriterionService.mutate
  /// </summary>
  public class DeletePlacement : ExampleBase {
    /// <summary>
    /// Main method, to run this code example as a standalone application.
    /// </summary>
    /// <param name="args">The command line arguments.</param>
    public static void Main(string[] args) {
      DeletePlacement codeExample = new DeletePlacement();
      Console.WriteLine(codeExample.Description);
      try {
        long adGroupId = long.Parse("INSERT_ADGROUP_ID_HERE");
        long placementId = long.Parse("INSERT_PLACEMENT_ID_HERE");
        codeExample.Run(new AdWordsUser(), adGroupId, placementId);
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
        return "This code example deletes a placement using the 'REMOVE' operator. To get " +
            "placements, run GetPlacements.cs.";
      }
    }

    /// <summary>
    /// Runs the code example.
    /// </summary>
    /// <param name="user">The AdWords user.</param>
    /// <param name="adGroupId">Id of the ad group that contains the placement.
    /// </param>
    /// <param name="placementId">Id of the placement to be deleted.</param>
    public void Run(AdWordsUser user, long adGroupId, long placementId) {
      // Get the AdGroupCriterionService.
      AdGroupCriterionService adGroupCriterionService = (AdGroupCriterionService)user.GetService(
          AdWordsService.v201309.AdGroupCriterionService);

      // Create base class criterion to avoid setting placement-specific fields.
      Criterion criterion = new Criterion();
      criterion.id = placementId;

      // Create the ad group criterion.
      BiddableAdGroupCriterion adGroupCriterion = new BiddableAdGroupCriterion();
      adGroupCriterion.adGroupId = adGroupId;
      adGroupCriterion.criterion = criterion;

      // Create the operation.
      AdGroupCriterionOperation operation = new AdGroupCriterionOperation();
      operation.operand = adGroupCriterion;
      operation.@operator = Operator.REMOVE;

      try {
        // Delete the placement.
        AdGroupCriterionReturnValue retVal = adGroupCriterionService.mutate(
            new AdGroupCriterionOperation[] {operation});

        // Display the results.
        if (retVal != null && retVal.value != null && retVal.value.Length > 0) {
          AdGroupCriterion deletedPlacement = retVal.value[0];
          Console.WriteLine("Placement with ad group id = \"{0}\" and id = \"{1}\" was deleted.",
              deletedPlacement.adGroupId, deletedPlacement.criterion.id);
        } else {
          Console.WriteLine("No placement was deleted.");
        }
      } catch (Exception ex) {
        throw new System.ApplicationException("Failed to delete placement.", ex);
      }
    }
  }
}

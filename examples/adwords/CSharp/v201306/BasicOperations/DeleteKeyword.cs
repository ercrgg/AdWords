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
  /// This code example deletes a keyword using the 'REMOVE' operator. To get
  /// keywords, run GetKeywords.cs.
  ///
  /// Tags: AdGroupCriterionService.mutate
  /// </summary>
  public class DeleteKeyword : ExampleBase {
    /// <summary>
    /// Main method, to run this code example as a standalone application.
    /// </summary>
    /// <param name="args">The command line arguments.</param>
    public static void Main(string[] args) {
      DeleteKeyword codeExample = new DeleteKeyword();
      Console.WriteLine(codeExample.Description);
      try {
        long adGroupId = long.Parse("INSERT_ADGROUP_ID_HERE");
        long keywordId = long.Parse("INSERT_KEYWORD_ID_HERE");
        codeExample.Run(new AdWordsUser(), adGroupId, keywordId);
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
        return "This code example deletes a keyword using the 'REMOVE' operator. To get " +
            "keywords, run GetKeywords.cs.";
      }
    }

    /// <summary>
    /// Runs the code example.
    /// </summary>
    /// <param name="user">The AdWords user.</param>
    /// <param name="adGroupId">Id of the ad group that contains the keyword.
    /// </param>
    /// <param name="keywordId">Id of the keyword to be deleted.</param>
    public void Run(AdWordsUser user, long adGroupId, long keywordId) {
      // Get the AdGroupCriterionService.
      AdGroupCriterionService adGroupCriterionService = (AdGroupCriterionService)user.GetService(
          AdWordsService.v201306.AdGroupCriterionService);

      // Create base class criterion to avoid setting keyword-specific fields.
      Criterion criterion = new Criterion();
      criterion.id = keywordId;

      // Create the ad group criterion.
      BiddableAdGroupCriterion adGroupCriterion = new BiddableAdGroupCriterion();
      adGroupCriterion.adGroupId = adGroupId;
      adGroupCriterion.criterion = criterion;

      // Create the operation.
      AdGroupCriterionOperation operation = new AdGroupCriterionOperation();
      operation.operand = adGroupCriterion;
      operation.@operator = Operator.REMOVE;

      try {
        // Delete the keyword.
        AdGroupCriterionReturnValue retVal = adGroupCriterionService.mutate(
            new AdGroupCriterionOperation[] {operation});

        // Display the results.
        if (retVal != null && retVal.value != null && retVal.value.Length > 0) {
          AdGroupCriterion deletedKeyword = retVal.value[0];
          Console.WriteLine("Keyword with ad group id = \"{0}\" and id = \"{1}\" was deleted.",
              deletedKeyword.adGroupId, deletedKeyword.criterion.id);
        } else {
          Console.WriteLine("No keywords were deleted.");
        }
      } catch (Exception ex) {
        throw new System.ApplicationException("Failed to delete keyword.", ex);
      }
    }
  }
}

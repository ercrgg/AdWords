﻿// Copyright 2013, Google Inc. All Rights Reserved.
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

using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

using CSharpExamples = Google.Api.Ads.AdWords.Examples.CSharp.v201309;
using VBExamples = Google.Api.Ads.AdWords.Examples.VB.v201309;

namespace Google.Api.Ads.AdWords.Tests.v201309 {
  /// <summary>
  /// Test cases for all the code examples under v201309\Optimization.
  /// </summary>
  class OptimizationTest : VersionedExampleTestsBase {
    long campaignId;
    long adGroupId;

    /// <summary>
    /// Inits this instance.
    /// </summary>
    [SetUp]
    public void Init() {
      campaignId = utils.CreateCampaign(user, BiddingStrategyType.MANUAL_CPM);
      adGroupId = utils.CreateAdGroup(user, campaignId, true);
    }

    /// <summary>
    /// Tests the GetPlacementIdeas VB.NET code example.
    /// </summary>
    [Test]
    public void TestGetPlacementIdeasVBExample() {
      RunExample(delegate() {
        new VBExamples.GetPlacementIdeas().Run(user);
      });
    }

    /// <summary>
    /// Tests the GetPlacementIdeas C# code example.
    /// </summary>
    [Test]
    public void TestGetPlacementIdeasCSharpExample() {
      RunExample(delegate() {
        new CSharpExamples.GetPlacementIdeas().Run(user);
      });
    }
  }
}

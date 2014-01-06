﻿// Copyright 2011, Google Inc. All Rights Reserved.
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

using System;
using System.Collections.Generic;
using System.Text;

namespace Google.Api.Ads.Common.Lib {
  /// <summary>
  /// Default implementation of auth token cache.
  /// </summary>
  [Obsolete("ClientLogin API is deprecated, see " +
      "https://developers.google.com/accounts/docs/AuthForInstalledApps for details. OAuth2 is " +
      "the recommended authentication mechanism. You can refer to the code examples for details " +
      " on how to use OAUth2 in your application. You can use Util\\OAuth2TokenGenerator.cs for " +
      "generating OAuth2 refresh tokens for offline access to various Ads* APIs.")]
  public class DefaultAuthTokenCache : AuthTokenCache {
    /// <summary>
    /// An internal dictionary for caching auth tokens.
    /// </summary>
    InMemoryTokenCache<string> tokenCache = new InMemoryTokenCache<string>();

    #region AuthTokenCache Members

    /// <summary>
    /// Adds an auth token to cache.
    /// </summary>
    /// <param name="service">The ClientLogin service for which this auth token
    /// is generated.</param>
    /// <param name="email">The login email.</param>
    /// <param name="password">The login password.</param>
    /// <param name="token">The auth token.</param>
    /// <returns>
    /// The auth token.
    /// </returns>
    public string AddToken(string service, string email, string password, string token) {
      return AddToken(service, email, token);
    }

    /// <summary>
    /// Gets an auth token from cache.
    /// </summary>
    /// <param name="service">The ClientLogin service for which this auth token
    /// is generated.</param>
    /// <param name="email">The login email.</param>
    /// <param name="password">The login password.</param>
    /// <returns>
    /// The auth token, or null if the cache doesn't have a token.
    /// </returns>
    public string GetToken(string service, string email, string password) {
      return GetToken(service, email);
    }


    /// <summary>
    /// Adds an auth token to cache.
    /// </summary>
    /// <param name="service">The ClientLogin service for which this auth token
    /// is generated.</param>
    /// <param name="email">The login email.</param>
    /// <param name="token">The auth token.</param>
    /// <returns>The auth token.</returns>
    public string AddToken(string service, string email, string token) {
      ValidateParams(service, email);
      return tokenCache.AddToken(GetKey(service, email), token);
    }

    /// <summary>
    /// Gets an auth token from cache.
    /// </summary>
    /// <param name="service">The ClientLogin service for which this auth token
    /// is generated.</param>
    /// <param name="email">The login email.</param>
    /// <returns>The auth token, or null if the cache doesn't have a token.
    /// </returns>
    public string GetToken(string service, string email) {
      ValidateParams(service, email);
      return tokenCache.GetToken(GetKey(service, email));
    }

    /// <summary>
    /// Invalidates an auth token.
    /// </summary>
    /// <param name="token">The auth token.</param>
    public void InvalidateToken(string token) {
      tokenCache.InvalidateToken(token);
    }

    /// <summary>
    /// Clears the cache.
    /// </summary>
    public void Clear() {
      tokenCache.Clear();
    }

    #endregion

    /// <summary>
    /// Gets a unique key for cache map.
    /// </summary>
    /// <param name="service">The ClientLogin service.</param>
    /// <param name="email">The login email.</param>
    /// <returns>The cache map key.</returns>
    private string GetKey(string service, string email) {
      return string.Format("{0}:{1}", service, email.ToLower());
    }

    /// <summary>
    /// Validates the input params.
    /// </summary>
    /// <param name="service">The ClientLogin service name.</param>
    /// <param name="email">The login email.</param>
    private static void ValidateParams(string service, string email) {
      if (string.IsNullOrEmpty(email)) {
        throw new ArgumentException(CommonErrorMessages.EmailCannotBeNull);
      }
      if (string.IsNullOrEmpty(service)) {
        throw new ArgumentException(CommonErrorMessages.ServiceCannotBeNull);
      }
    }
  }
}

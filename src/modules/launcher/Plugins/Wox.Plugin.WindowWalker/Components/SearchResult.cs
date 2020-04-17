﻿// Copyright (c) Microsoft Corporation
// The Microsoft Corporation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.  Code forked from Betsegaw Tadele's https://github.com/betsegaw/windowwalker/

using System.Collections.Generic;

namespace Wox.Plugin.WindowWalker.Components
{
    /// <summary>
    /// Contains search result windows with each window including the reason why the result was included
    /// </summary>
    public class SearchResult
    {
        /// <summary>
        /// Gets the actual window reference for the search result
        /// </summary>
        public Window Result
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the list of indexes of the matching characters for the search in the title window
        /// </summary>
        public List<int> SearchMatchesInTitle
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the list of indexes of the matching characters for the search in the
        /// name of the process
        /// </summary>
        public List<int> SearchMatchesInProcessName
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the type of match (shortcut, fuzzy or nothing)
        /// </summary>
        public SearchType SearchResultMatchType
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a score indicating how well this matches what we are looking for
        /// </summary>
        public int Score
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the source of where the best score was found
        /// </summary>
        public TextType BestScoreSource
        {
            get;
            private set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchResult"/> class.
        /// Constructor
        /// </summary>
        public SearchResult(Window window, List<int> matchesInTitle, List<int> matchesInProcessName, SearchType matchType)
        {
            Result = window;
            SearchMatchesInTitle = matchesInTitle;
            SearchMatchesInProcessName = matchesInProcessName;
            SearchResultMatchType = matchType;
            CalculateScore();
        }

        /// <summary>
        /// Calculates the score for how closely this window matches the search string
        /// </summary>
        /// <remarks>
        /// Higher Score is better
        /// </remarks>
        private void CalculateScore()
        {
            if (FuzzyMatching.CalculateScoreForMatches(SearchMatchesInProcessName) >
                FuzzyMatching.CalculateScoreForMatches(SearchMatchesInTitle))
            {
                Score = FuzzyMatching.CalculateScoreForMatches(SearchMatchesInProcessName);
                BestScoreSource = TextType.ProcessName;
            }
            else
            {
                Score = FuzzyMatching.CalculateScoreForMatches(SearchMatchesInTitle);
                BestScoreSource = TextType.WindowTitle;
            }
        }

        /// <summary>
        /// The type of text that a string represents
        /// </summary>
        public enum TextType
        {
            ProcessName,
            WindowTitle,
        }

        /// <summary>
        /// The type of search
        /// </summary>
        public enum SearchType
        {
            /// <summary>
            /// the search string is empty, which means all open windows are
            /// going to be returned
            /// </summary>
            Empty,

            /// <summary>
            /// Regular fuzzy match search
            /// </summary>
            Fuzzy,

            /// <summary>
            /// The user has entered text that has been matched to a shortcut
            /// and the shortcut is now being searched
            /// </summary>
            Shortcut,
        }
    }
}

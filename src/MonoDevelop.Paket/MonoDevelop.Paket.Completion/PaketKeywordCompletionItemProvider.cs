﻿//
// PaketKeywordCompletionItemProvider.cs
//
// Author:
//       Matt Ward <ward.matt@gmail.com>
//
// Copyright (c) 2015 Matthew Ward
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//

using MonoDevelop.Ide.CodeCompletion;

namespace MonoDevelop.Paket.Completion
{
	public class PaketKeywordCompletionItemProvider
	{
		CompletionDataList completionData;

		public ICompletionDataList GenerateCompletionItems ()
		{
			completionData = new CompletionDataList ();
			AddSettings ("content");
			AddSettings ("copy_local");
			AddSettings ("framework");
			Add ("gist");
			Add ("github");
			Add ("http");
			AddSettings ("import_targets");
			Add ("nuget");
			Add ("source");
			AddSettings ("redirects");
			AddSettings ("references");
			completionData.IsSorted = true;
			return completionData;
		}

		void AddSettings (string name)
		{
			Add (name, isSettings: true);
		}

		void Add (string name, bool isSettings = false)
		{
			Add (name, GetCompletionText (name, isSettings));
		}

		string GetCompletionText (string name, bool isSettings)
		{
			if (isSettings)
				return name + ":";
			return name;
		}

		void Add (string name, string completionText)
		{
			completionData.Add (new CompletionData (name, null, null, completionText));
		}
	}
}


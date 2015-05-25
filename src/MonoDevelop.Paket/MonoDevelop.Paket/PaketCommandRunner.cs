﻿//
// PaketCommandRunner.cs
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

using System;
using MonoDevelop.Core;
using MonoDevelop.Core.Execution;
using MonoDevelop.Core.ProgressMonitoring;

namespace MonoDevelop.Paket
{
	public class PaketCommandRunner
	{
		public void Run (PaketCommandLine command, ProgressMonitorStatusMessage progressMessage)
		{
			AggregatedProgressMonitor progressMonitor = CreateProgressMonitor (progressMessage);
			try {
				Run (command, progressMonitor, progressMessage);
			} catch (Exception ex) {
				LoggingService.LogError ("Error running paket command", ex);
				progressMonitor.Log.WriteLine (ex.Message);
				progressMonitor.ReportError (progressMessage.Error, null);
				PaketConsolePad.Show (progressMonitor);
				progressMonitor.Dispose ();
			}
		}

		AggregatedProgressMonitor CreateProgressMonitor (ProgressMonitorStatusMessage progressMessage)
		{
			var factory = new ProgressMonitorFactory ();
			return (AggregatedProgressMonitor)factory.CreateProgressMonitor (progressMessage.Status);
		}

		void Run (PaketCommandLine commandLine, AggregatedProgressMonitor progressMonitor, ProgressMonitorStatusMessage progressMessage)
		{
			progressMonitor.Log.WriteLine (commandLine);
			IProcessAsyncOperation operation = Runtime.ProcessService.StartConsoleProcess (
				commandLine.Command,
				commandLine.Arguments,
				commandLine.WorkingDirectory,
				progressMonitor.MasterMonitor as IConsole,
				(sender, e) => {
					using (progressMonitor) {
						OnCommandCompleted ((IAsyncOperation)sender, progressMonitor, progressMessage);
					}
				}
			);
		}

		void OnCommandCompleted (
			IAsyncOperation operation,
			AggregatedProgressMonitor progressMonitor,
			ProgressMonitorStatusMessage progressMessage)
		{
			ReportOutcome (operation, progressMonitor, progressMessage);
		}

		void ReportOutcome (
			IAsyncOperation operation,
			AggregatedProgressMonitor progressMonitor,
			ProgressMonitorStatusMessage progressMessage)
		{
			if (operation.Success) {
				progressMonitor.ReportSuccess (progressMessage.Success);
			} else {
				progressMonitor.ReportError (progressMessage.Error, null);
				PaketConsolePad.Show (progressMonitor);
			}
		}
	}
}

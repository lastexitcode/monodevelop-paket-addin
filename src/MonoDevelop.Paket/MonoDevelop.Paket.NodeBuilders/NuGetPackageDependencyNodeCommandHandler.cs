﻿//
// NuGetPackageDependencyNodeCommandHandler.cs
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
using MonoDevelop.Components.Commands;
using MonoDevelop.Core;
using MonoDevelop.Ide.Commands;
using MonoDevelop.Ide.Gui.Components;
using MonoDevelop.Paket.Commands;

namespace MonoDevelop.Paket.NodeBuilders
{
	public class NuGetPackageDependencyNodeCommandHandler : NodeCommandHandler
	{
		[CommandUpdateHandler (EditCommands.Delete)]
		public void UpdateRemoveItem (CommandInfo info)
		{
			info.Enabled = CanDeleteMultipleItems ();
			info.Text = GettextCatalog.GetString ("Remove");
		}

		public override bool CanDeleteMultipleItems ()
		{
			return !MultipleSelectedNodes;
		}

		public override void DeleteItem ()
		{
			ProgressMonitorStatusMessage progressMessage = ProgressMonitorStatusMessageFactory.CreateRemoveNuGetPackageMessage (DependencyNode.Id);

			try {
				RemoveDependency (progressMessage);
			} catch (Exception ex) {
				PaketServices.ActionRunner.ShowError (progressMessage, ex);
			}
		}

		NuGetPackageDependencyNode DependencyNode {
			get { return (NuGetPackageDependencyNode)CurrentNode.DataItem; }
		}

		void RemoveDependency (ProgressMonitorStatusMessage progressMessage)
		{
			var action = new RemoveNuGetPaketAction (
				DependencyNode.Id,
				DependencyNode.GetPackageDependencyFile ());
			PaketServices.ActionRunner.Run (progressMessage, action);
		}

		[CommandHandler (PaketCommands.UpdatePackage)]
		public void Update ()
		{
			var message = ProgressMonitorStatusMessageFactory.CreateUpdateNuGetPackageMessage (DependencyNode.Id);
			var action = new UpdateNuGetPaketAction (
				DependencyNode.Id,
				DependencyNode.GetPackageDependencyFile ());
			PaketServices.ActionRunner.Run (message, action);
		}
	}
}


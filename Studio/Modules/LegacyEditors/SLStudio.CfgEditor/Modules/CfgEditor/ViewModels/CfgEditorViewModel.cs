﻿using ICSharpCode.AvalonEdit.Document;
using SLStudio.CfgEditor.Resources;
using SLStudio.Core;
using System;
using System.IO;
using System.Threading.Tasks;

namespace SLStudio.CfgEditor
{
    [FileEditor("editorName", "editorDescription", "editorCategory", typeof(CfgEditorResources), "", false, ".cfg")]
    internal class CfgEditorViewModel : FileDocumentBase, ICfgEditor
    {
        private string lastCheckpoint;
        private int busyOperations = 0;

        public CfgEditorViewModel()
        {
            TextDocument = new TextDocument();
            TextDocument.TextChanged += OnTextChanged;
        }

        public bool IsBusy => busyOperations > 0;

        public TextDocument TextDocument { get; }

        public string Content
        {
            get => TextDocument.Text;
            set => TextDocument.Text = value;
        }

        protected override Task DoNew(string content)
        {
            Content = content;

            return Task.CompletedTask;
        }

        protected override async Task DoLoadFrom(Stream stream)
        {
            Busy();

            using var streamReader = new StreamReader(stream);
            Content = await streamReader.ReadToEndAsync();

            SetCheckpoint(Content);

            Idle();
        }

        protected override async Task DoSaveTo(Stream stream)
        {
            Busy();

            using var streamWriter = new StreamWriter(stream, leaveOpen: true);
            await streamWriter.WriteAsync(Content);
            await streamWriter.FlushAsync();

            SetCheckpoint(Content);

            Idle();
        }

        private void Busy()
        {
            ++busyOperations;
            RaisePropertyChanged(() => IsBusy);
        }

        private void Idle()
        {
            --busyOperations;
            RaisePropertyChanged(() => IsBusy);
        }

        private void SetCheckpoint(string content)
        {
            lastCheckpoint = content;
            UpdateIsDirty();
        }

        private void UpdateIsDirty()
        {
            if (IsNew || string.IsNullOrEmpty(FileName))
            {
                IsDirty = true;
                return;
            }

            if (string.IsNullOrEmpty(lastCheckpoint))
                lastCheckpoint = string.Empty;

            IsDirty = !lastCheckpoint.Equals(Content);
        }

        private void OnTextChanged(object sender, EventArgs e)
        {
            UpdateIsDirty();
        }
    }

    public interface ICfgEditor
    {
    }
}
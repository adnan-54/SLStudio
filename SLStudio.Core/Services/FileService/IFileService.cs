﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace SLStudio.Core
{
    public interface IFileService
    {
        bool CanEdit(string extension);

        IEnumerable<IFileDescription> GetFileDescriptions();

        Task<IFileDocumentPanel> Empty(string extension);

        Task<IFileDocumentPanel> New(string extension, string displayName = null, string content = null);

        Task<T> New<T>(string displayName = null, string content = null) where T : class, IFileDocumentPanel;

        string GetFilter(string extension);

        //Task<IFileDocumentPanel> Open(string fileName);

        //Task<T> Open<T>(string fileName) where T : class, IFileDocumentPanel;

        //Task<bool> Save(IFileDocumentPanel file);

        //Task<bool> SaveAs(IFileDocumentPanel file);
    }
}
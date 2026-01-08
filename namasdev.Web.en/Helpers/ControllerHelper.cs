using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using namasdev.Core.Types;
using namasdev.Core.Validation;

namespace namasdev.Web.Helpers
{
    public class ControllerHelper
    {
        private const string HTML_LINE_BREAK = "<br/>";
        private readonly Controller _controller;

        public ControllerHelper(Controller controller)
        {
            Validator.ValidateRequiredArgumentAndThrow(controller, nameof(controller));

            _controller = controller;
        }

        public void SetMessageSuccess(params string[] messages)
        {
            ViewBagHelper.SetMessageSuccess(_controller.ViewBag, messages);
        }

        public void SetMessageInfo(params string[] messages)
        {
            ViewBagHelper.SetMessageInfo(_controller.ViewBag, messages);
        }

        public void SetMessageWarning(params string[] messages)
        {
            ViewBagHelper.SetMessageWarning(_controller.ViewBag, messages);
        }

        public void SetMessageError(params string[] messages)
        {
            ViewBagHelper.SetMessageError(_controller.ViewBag, messages);
        }

        public ActionResult CreateFileActionResult(string fileName, byte[] fileContent,
            bool download = true,
            string downloadFileName = null)
        {
            var result = new FileContentResult(fileContent, MimeMapping.GetMimeMapping(fileName));
            if (download)
            {
                result.FileDownloadName = 
                    !string.IsNullOrWhiteSpace(downloadFileName)
                    ? downloadFileName
                    : Path.GetFileName(fileName);
            }
            return result;
        }

        public ActionResult CreateFileActionResult(string filePath,
            bool download = true,
            string downloadFileName = null)
        {
            return CreateFileActionResult(filePath, File.ReadAllBytes(filePath), 
                download,
                downloadFileName);
        }

        public ActionResult CreateFileActionResult(Core.IO.File file,
            bool download = true,
            string downloadFileName = null)
        {
            return CreateFileActionResult(file.Name, file.Content, 
                download,
                downloadFileName);
        }

        public byte[] GetPostedFileAsBytes(HttpPostedFileBase postedFile)
        {
            if (postedFile == null || postedFile.ContentLength == 0)
            {
                throw new Exception(Validator.Messages.Required("File"));
            }

            return GetPostedFileBytes(postedFile);
        }

        public Core.IO.File GetPostedFileAsFile(HttpPostedFileBase postedFile, 
            string description = null,
            bool required = true)
        {
            if (postedFile == null || postedFile.ContentLength == 0)
            {
                if (required)
                {
                    throw new Exception(Validator.Messages.Required(description));
                }
                else
                {
                    return null;
                }
            }

            return CreateFileFromPostedFile(postedFile);
        }

        public Core.IO.File[] GetPostedFilesAsFiles(HttpPostedFileBase[] postedFiles, string description,
            bool required = true)
        {
            var validList = postedFiles?
                .Where(pf => pf != null && pf.ContentLength != 0)
                .Select(pf => CreateFileFromPostedFile(pf))
                .ToArray();

            if (validList == null || !postedFiles.Any())
            {
                if (required)
                {
                    throw new Exception(Validator.Messages.ListNotEmpty(description));
                }
                else
                {
                    return null;
                }
            }

            return validList;
        }

        private Core.IO.File CreateFileFromPostedFile(HttpPostedFileBase postedFile)
        {
            return
                postedFile != null
                ? new Core.IO.File { Name = postedFile.FileName, Content = GetPostedFileBytes(postedFile) }
                : null;
        }

        private byte[] GetPostedFileBytes(HttpPostedFileBase postedFile)
        {
            if (postedFile == null)
            {
                return null;
            }

            var bytes = new byte[postedFile.ContentLength];
            postedFile.InputStream.Read(bytes, 0, bytes.Length);
            return bytes;
        }

        public string GetModelStateErrorMessages()
        {
            return Formatter.List(GetModelStateErrorMessageList(), HTML_LINE_BREAK);
        }

        public void GroupModelStateErrors()
        {
            var elements = GetModelStateErrorElements(membersErrorsOnly: true);

            var messages = new List<string>();
            foreach (var e in elements)
            {
                foreach (var em in GetModelStateErrorMessageList(e))
                {
                    if (!messages.Contains(em))
                    {
                        messages.Add(em);

                        _controller.ModelState.AddModelError("", em);
                    }
                }

                e.Errors.Clear();
            }
        }

        private IEnumerable<string> GetModelStateErrorMessageList(
            bool membersErrorsOnly = false)
        {
            return GetModelStateErrorElements(membersErrorsOnly)
                .SelectMany(GetModelStateErrorMessageList)
                .ToArray();
        }

        private IEnumerable<ModelState> GetModelStateErrorElements(
            bool membersErrorsOnly = false)
        {
            return _controller.ModelState
                .Where(ms =>
                    ms.Value.Errors.Count > 0
                    && (!membersErrorsOnly || !string.IsNullOrWhiteSpace(ms.Key)))
                .Select(ms => ms.Value)
                .ToList();
        }

        private IEnumerable<string> GetModelStateErrorMessageList(ModelState element)
        {
            return element.Errors.Select(e => e.ErrorMessage).ToArray();
        }
    }
}
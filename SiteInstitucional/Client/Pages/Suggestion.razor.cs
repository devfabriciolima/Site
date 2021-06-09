using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Logging;

using SiteInstitucional.Client.Api;
using SiteInstitucional.Client.Shared;
using SiteInstitucional.Shared.Domain;

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace SiteInstitucional.Client.Pages
{
    public partial class Suggestion
    {
        [Inject]
        public IIdeaApi IdeaApi { get; set; }

        [Inject]
        private IDepartmentApi DepartmentApi { get; set; }

        [Inject]
        private ILogger<Suggestion> Logger { get; set; }

        private Idea _model = new();
        private List<Department> _listDepartments = new();
        private MessageModal _messageModal;

        private bool _sendingMessage;

        private const long _maxFileSizeMb = 15;
        private const long _maxFileSize = _maxFileSizeMb * 1024 * 1024;
        private const int _maxAllowedFiles = 10;
        private bool _sendingFile;
        private string _exceptionMessage;

        protected override async Task OnInitializedAsync()
        {
            var response = await DepartmentApi.GetDepartments();
            if (response.IsSuccessStatusCode)
            {
                _listDepartments = response.Content;
                return;
            }
            Logger.LogError(response.Error, "An error occured while fetching department list.");
        }

        private async Task SendMail()
        {
            _sendingMessage = true;
            await IdeaApi.SendSuggestionOfIdea(_model);
            _messageModal.Message = Localizer["Mensagem enviada com sucesso!"];
            _messageModal.Open();
            _model = new Idea();
            _sendingMessage = false;
        }

        private async Task LoadFiles(InputFileChangeEventArgs e)
        {
            _sendingFile = true;
            _model.Attachments.Clear();
            _exceptionMessage = string.Empty;
            try
            {
                foreach (var file in e.GetMultipleFiles(_maxAllowedFiles))
                {
                    using var reader = new StreamReader(file.OpenReadStream(_maxFileSize));
                    await using var memoryStream = new MemoryStream();
                    await reader.BaseStream.CopyToAsync(memoryStream);
                    _model.Attachments.Add(new IdeaAttachment(file.Name, file.ContentType, memoryStream.ToArray()));
                }
            }
            catch (InvalidOperationException ex)
            {
                _exceptionMessage = $"O limite de ${_maxAllowedFiles} foi excedido.";
                Logger.LogError(ex, "An error occured while attach file.");
            }
            catch (IOException ex)
            {
                _exceptionMessage = $"O limite de ${_maxFileSizeMb}MB por arquivo foi excedido.";
                Logger.LogError(ex, "An error occured while attach file.");
            }
            catch (Exception ex)
            {
                _exceptionMessage = ex.Message;
                Logger.LogError(ex, "An error occured while attach file.");
            }
            _sendingFile = false;
        }
    }
}

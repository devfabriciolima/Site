using System;
using System.Data;
using System.Threading.Tasks;

using Dapper;

using SiteInstitucional.Shared.Domain;

namespace SiteInstitucional.Server.Core.Repositories
{
    public class IdeaRepository
    {
        private readonly IDbConnection _db;

        public IdeaRepository(IDbConnection db)
        {
            _db = db;
        }

        public async Task Save(Idea idea)
        {
            const string insertIdeaCommand = "INSERT INTO [dbo].[IDEIA] "
                                         + "([SetorAplicacao], [MatriculaColaborador], [NomeColaborador] "
                                         + ", [SetorColaborador], [MinhaIdeia], [AplicacaoIdeia], [MedicaoIdeia]) "
                                         + "VALUES(@Department, @CollaboratorRegistration, @CollaboratorName "
                                         + ", @CollaboratorDepartment, @MyIdea, @WhyApplicable, @HowMeasured); "
                                         + "SELECT SCOPE_IDENTITY()";
            const string insertAttachmentCommand = "INSERT INTO [dbo].[IDEIA_ANEXO] "
                                                   + "([CodigoIdeia], [NomeArquivo], [TipoConteudo], [Arquivo]) "
                                                   + "VALUES(@IdeaId, @Filename, @ContentType, @Bytes)";
            _db.Open();
            using var transaction = _db.BeginTransaction();
            try
            {
                var insertedId = await _db.ExecuteScalarAsync<int?>(insertIdeaCommand, idea, transaction);
                if (insertedId > 0 && idea.Attachments != null)
                {
                    foreach (var attachment in idea.Attachments)
                    {
                        attachment.IdeaId = insertedId.Value;
                        await _db.ExecuteAsync(insertAttachmentCommand, attachment, transaction);
                    }
                }

                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
            finally
            {
                if (_db.State == ConnectionState.Open)
                {
                    _db.Close();
                }
            }
        }
    }
}

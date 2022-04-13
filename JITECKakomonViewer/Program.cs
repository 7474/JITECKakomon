using System.Threading.Tasks;
using JITECEntity;
using Microsoft.Extensions.DependencyInjection;
using Statiq.App;
using Statiq.Common;
using Statiq.Web;

namespace JITECKakomonViewer
{
    public class Program
    {
        public static async Task<int> Main(string[] args) =>
          await Bootstrapper
            .Factory
            .CreateDefault(args)
            .AddHostingCommands()
            .ConfigureServices(services =>
            {
                // XXX DIとパイプラインとでInパラ整理
                services.AddSingleton<IRepository>(s => new Repository(
                    new HttpClient(),
                    "https://ipakakomon.blob.core.windows.net/ipa-kakomon"));
            })
            .AddPipeline<ExamPartPipeline>(typeof(ExamPartPipeline).Name)
            .AddPipeline<ExamPartIndexPipeline>(typeof(ExamPartIndexPipeline).Name)
            .RunAsync();
    }
}
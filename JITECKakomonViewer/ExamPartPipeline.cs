using JITECEntity;
using Statiq.Common;
using Statiq.Core;
using Statiq.Razor;

namespace JITECKakomonViewer
{
    public class ExamPartPipeline : Pipeline
    {
        public ExamPartPipeline(IRepository repository)
        {
            InputModules = new ModuleList
            {
                new ReadExamParts(new NormalizedPath("input/exam-part-index.json"), repository)
            };

            ProcessModules = new ModuleList
            {
                new MergeContent(new ReadFiles("_ExamPart.cshtml")),
                new RenderRazor().WithModel(Config.FromDocument((doc, context) =>
                {
                    return doc.Get<ExamPartViewModel>("Model");
                })),
                new SetDestination(Config.FromDocument((doc, ctx) =>
                {
                    var vm =  doc.Get<ExamPartViewModel>("Model");
                    return new NormalizedPath($"exam/{vm.ExamPartResult.ExamPart.ExamPartId}.html");
                }))
            };

            OutputModules = new ModuleList {
                  new WriteFiles()
            };
        }
    }
}

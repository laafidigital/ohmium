using Ohmium.Models.TemplateModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ohmium.Models.EFModels.ViewModels
{
    public class ScriptViewModel
    {
        [ForeignKey("div")]
        public string testStandID { get; set; }
        public Device div { get; set; }
        [ForeignKey("stk")]
        public string stackID { get; set; }
        public Stack stk { get; set; }
        public int scriptID { get; set; }
        public int MEA { get; set; }
        public List<scripStepViewModel> Scripts { get; set; }
    }

    public class scripStepViewModel
    {
        public int scriptID { get; set; }
        public int stepNo { get; set; }
        public int stepLoop { get; set; }
        public List<RunStepListViewModel> runstepListViewModel { get; set; }

    }

    public class RunStepListViewModel
    {
        public List<RunStepLibrary> runsteps { get; set; }
        public int stepNo { get; set; }
        public int stepLoop { get; set; }
    }

    public class StackSynceViewModel
    {
        public Stack stk { get; set; }
        public List<ScriptLibrary> scriptLib { get; set; }
    }
}

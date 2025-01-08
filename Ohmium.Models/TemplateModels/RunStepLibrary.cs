using Ohmium.Models.EFModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Ohmium.Models.TemplateModels
{
    public class SequenceLibrary
    {
        public int id { get; set; }
        public string sequenceName { get; set; }
        public int? loopCount { get; set; }
        public int sortOrder { get; set; }

        [NotMapped]
        public bool check { get; set; } = false;
    }
    public class RunStepLibrary
    {
        public int id { get; set; }
        [ForeignKey("seqmaster")]
        public int? seqMasterId { get; set; }
        public SequenceLibrary seqmaster { get; set; }

        [Display(Name = "Run Step NO")]
        public int stepNumber { get; set; }
        [Display(Name = "Duration")]
        public int duration { get; set; }
        public float? cI { get; set; } //Current set point
        public float? cV { get; set; } //Voltage set point
        public float? wP { get; set; } //Water back pressure set point
        public float? hP { get; set; } //Hydrogen back pressure set point
        public float? wFt { get; set; } //Water flow threshold
        public float? wTt { get; set; } //Water out temperature threshold
        public float? cVt { get; set; } //Cell voltage threshold
        public float? cVlimit { get; set; } //Cell voltage Limit

        public float? mnF { get; set; } = 0; //minimum frequency for sweep
        public float? mxF { get; set; } = 0;//maximum frequency for sweep
        public float? imF { get; set; }
        public float? imA { get; set; }
    }
    
    public class ScriptList
    {
        public int id { get; set; }
        public string scriptName { get; set; }
    }
    public class ScriptLibrary
    {
        public int id { get; set; }
        [ForeignKey("script")]
        public int scriptId { get; set; }
        public ScriptList script { get; set; }
        [Display(Name = "Script Library Step NO")]
        public int stepNumber{ get; set; }
        [Display(Name = "Script Loop")]
        public int? phaseLoop { get; set; }
        [Display(Name = "Sequence Steps & its loop")]
        public string runStepLibraryWithLoop { get; set; }
        [NotMapped]
        public SequenceLibrary seqLib { get; set; }
        [ForeignKey("statusType")]
        public int status { get; set; }
        public StatusType statusType { get; set; }

    }

    public class StackSync
    {
        [Key,Column(Order = 1)]
        public string stackID { get; set; }
        [Key, Column(Order = 2)]
        [ForeignKey("script")]
        public int scriptID { get; set; }
        public ScriptList script { get; set; }
    }
}

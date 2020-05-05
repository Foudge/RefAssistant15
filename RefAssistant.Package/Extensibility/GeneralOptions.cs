using System.ComponentModel;
using Lardite.RefAssistant;

namespace RefAssistant.Extensibility
{
    internal class GeneralOptions : BaseOptionModel<GeneralOptions>, IExtensionOptions
    {
        [Category("General")]
        [DisplayName("Show Unused References Window before removing")]
        [Description("Show the window containing list of removable references. Each of these references can be excluded from removable references.")]
        [DefaultValue(true)]
        public bool? IsShowUnusedReferencesWindow { get; set; }

        [Category("General")]
        [DisplayName("Remove Unused Usings after removing")]
        [Description("Apply the Remove Unused Using operation to all project files.")]
        [DefaultValue(false)]
        public bool? IsRemoveUsingsAfterRemoving { get; set; }
    }
}

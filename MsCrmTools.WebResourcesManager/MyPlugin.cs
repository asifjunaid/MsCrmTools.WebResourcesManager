﻿using System.ComponentModel.Composition;
using XrmToolBox.Extensibility;
using XrmToolBox.Extensibility.Interfaces;

namespace MscrmTools.WebresourcesManager
{
    // Do not forget to update version number and author (company attribute) in AssemblyInfo.cs class
    // To generate Base64 string for Images below, you can use https://www.base64-image.de/
    [Export(typeof(IXrmToolBoxPlugin)),
        ExportMetadata("Name", "Webresources Manager"),
        ExportMetadata("Description", "Manage your webresources the Visual Studio way"),
        // Please specify the base64 content of a 32x32 pixels image
        ExportMetadata("SmallImageBase64", "iVBORw0KGgoAAAANSUhEUgAAACAAAAAgCAIAAAD8GO2jAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAAZdEVYdFNvZnR3YXJlAHBhaW50Lm5ldCA0LjAuMjHxIGmVAAADa0lEQVRIS+2USUxTURSGi2OIibrQxIXRoEwtRbAqnYAylmIoQwVKh9fSFkpBihBkFAxjIokgIlQDC1PdExONC7dERYmJCdodgjHiQIkrDAuC/uU+yuvrU4iRHX/O4vT2vv97795zDq90m7UD2FQ+gNlsJj+2Q38D/Bcwz2S2UCZfVmIoK9aVr4e9SFeOMFC2tW3/Ll6EvDM0elCVX302rXVP5BAr+EnXsenHmrxe79TUlMvlslqt5OEqI9VdUETyMVUuSVjiZeTWhoSPhEm6inT23RF3eKdHmBEp77DZbL8CNT09XVlZiYdr9MbXCWmlZt9hvhCnXTZQxJQpnpGyHhAM7Iu6rdWXARMMwPvSxgxNTExYLBYAPgvlnZpiAN6eS3mYmUO7MuS7ZLGyCV7xqS3ZGmdI+DALACPalaHl5eW6ujoA5oXyZ4lKmLw5nzITl1RmYteFD4Cb3Bs5tD96UG+0HYrt3woAGh0dJYBPQjku41FK9iJfeitHQ3z9ohtNkNwOO3l2fdLFehYA/9KWgRofHycA+Loz1W2F2u8C2aQ43RL4DTQgt7ByV/jwkbg+A2UN5Q9uBeB2u/2AmTOJFZTJI1J8E8gatHriSUQDzGbLMdENMECKT722KWBlZaWlpcUPQAyqNfey85E8VqiIJxENgJT5NXA8Ke7SaCv89RrxB4DH40F1MQGoV4eRmo+Rf42R4UqIJ7QBwFw4HHsT1oUldrw4AZySduEv2nVd6Ljm5masMwHwxeHg9ZHfV2403QYAEisbYSpUtBVoHTRAwgZ8mJ11Op1kPxOAeJKs6rikXRBI34sU/noNABTry/dFDR0U9qP7jsb3cQK83kWHw0H2swDzMbIavQHuC3xpl4YeIQEAiBxORm61TNWAJEzSjUXae10u112ymQVAPFCqx1R5SF6K08keNgCHg2ZOyLyKUYru4wTMzX1E92E9GDAtUlQZfPMDB9Wo1WEPGwAdv9ArzmoymcwnEno4Aaurqz29vVgPBnj5Uoymp8m+q0ZvYw8HIE1dK8lqwvxS5jk5AdDk5Csy7FgAxHNJ+tpVyzCaHJSJA4BmRk8gQSGI0luR0K4MLS39RC1xAvAR7YXadyLFlxjpgFrDAQiW3W6vCBK+AGPnik5fW8IOmwnfz1Wm26EdwKbaAWyi0tLfyWMx+aDmCUUAAAAASUVORK5CYII="),
        // Please specify the base64 content of a 80x80 pixels image
        ExportMetadata("BigImageBase64", "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"),
        ExportMetadata("BackgroundColor", "#606060"),
        ExportMetadata("PrimaryFontColor", "White"),
        ExportMetadata("SecondaryFontColor", "White")]
    public class MyPlugin : PluginBase
    {
        public override IXrmToolBoxPluginControl GetControl()
        {
            return new MyPluginControl();
        }
    }
}
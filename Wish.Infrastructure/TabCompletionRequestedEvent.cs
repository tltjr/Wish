using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.Events;

namespace Wish.Infrastructure
{
    public class TabCompletionRequestedEvent : CompositePresentationEvent<string> { }
}

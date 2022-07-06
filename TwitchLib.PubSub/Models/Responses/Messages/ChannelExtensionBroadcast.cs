using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using TwitchLib.PubSub.Common;

namespace TwitchLib.PubSub.Models.Responses.Messages
{
    /// <summary>
    /// VideoPlayback model constructor.
    /// Implements the <see cref="MessageData" />
    /// </summary>
    /// <seealso cref="MessageData" />
    /// <inheritdoc />
    public class ChannelExtensionBroadcast : MessageData
    {
        /// <summary>
        /// Video playback type
        /// </summary>
        /// <value>The messages.</value>
        public List<string> Messages { get; } = new List<string>();

        /// <summary>
        /// VideoPlayback constructor.
        /// </summary>
        /// <param name="jsonStr">The json string.</param>
        public ChannelExtensionBroadcast(string jsonStr)
        {
            var json = Helpers.ParseJson(jsonStr);
            foreach (var msg in json["content"])
                Messages.Add(msg.ToString());
        }
    }
}

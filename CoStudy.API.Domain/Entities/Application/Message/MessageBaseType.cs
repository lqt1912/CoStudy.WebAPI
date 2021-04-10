using System;
using System.Collections.Generic;
using System.Text;

namespace CoStudy.API.Domain.Entities.Application
{
    /// <summary>
    /// Enum MessageBaseType
    /// </summary>
    public enum MessageBaseType
    {
        /// <summary>
        /// The text
        /// </summary>
        Text,
        /// <summary>
        /// The image
        /// </summary>
        Image,
        /// <summary>
        /// The multi media
        /// </summary>
        MultiMedia,
        /// <summary>
        /// The post thumbnail
        /// </summary>
        PostThumbnail,

        /// <summary>
        /// The conversation activity
        /// </summary>
        ConversationActivity,

        /// <summary>
        /// The other
        /// </summary>
        Other
    }
}

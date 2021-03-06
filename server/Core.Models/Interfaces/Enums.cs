﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Models
{
    public enum VendorType
    {
        Photographer,
        Videographer,
        Florist,
        Caterer,
        Decor,
        DJ,
        MakeupArtist
    }

    public enum EventType
    {
        Wedding,
        Engagement,
        Corporate,
        BabyShower
    }

    public enum ArrangementStatus
    {
        ToBeStarted,
        InProgress,
        ReadyToReview,
        Complete
    }

}

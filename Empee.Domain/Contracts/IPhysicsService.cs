﻿namespace Empee.Domain.Contracts
{
    public interface IPhysicsService
    {
        float Gravity { get; set; }

        bool Visible { get; set; }
    }
}
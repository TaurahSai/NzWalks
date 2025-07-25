﻿namespace NzWalks.API.Model.DTO
{
    public class UpdateWalksRequestDto
    {
        public string Description { get; set; }
        public string Name { get; set; }
        public double LengthInKm { get; set; }
        public string? WalkImageUrl { get; set; }
        public Guid DifficultyId { get; set; }
        public Guid RegionId { get; set; }

    }
}

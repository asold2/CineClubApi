﻿namespace CineClubApi.Common.RequestBody;

public class AddMovieToListBody
{
    public Guid ListId { get; set; }
    public Guid UserId { get; set; }
    public int TmdbId { get; set; }
    
}
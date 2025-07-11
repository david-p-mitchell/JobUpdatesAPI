﻿using System.ComponentModel.DataAnnotations;

namespace JobUpdatesAPI.Models;

public class JobInterviewStageModel
{
    [Key]
    public short JobInterviewStageId { get; set; }
    public int CurrentStage {  get; set; } = 0; 
    public int InterviewStages { get; set; }

    public ICollection<JobStatusModel> JobStatuses { get; set; } = [];

    public JobStatusModel? JobUpdate { get; set; } = null!; // back to parent

    public override string ToString() =>  $"{JobInterviewStageId}: {CurrentStage}";
    
}

USE [master]
GO
/****** Object:  Database [LMPdev]    Script Date: 4/15/2020 4:17:47 PM ******/
CREATE DATABASE [LMPdev]

GO
ALTER DATABASE [LMPdev] SET COMPATIBILITY_LEVEL = 100
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [LMPdev].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [LMPdev] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [LMPdev] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [LMPdev] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [LMPdev] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [LMPdev] SET ARITHABORT OFF 
GO
ALTER DATABASE [LMPdev] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [LMPdev] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [LMPdev] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [LMPdev] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [LMPdev] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [LMPdev] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [LMPdev] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [LMPdev] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [LMPdev] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [LMPdev] SET  DISABLE_BROKER 
GO
ALTER DATABASE [LMPdev] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [LMPdev] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [LMPdev] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [LMPdev] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [LMPdev] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [LMPdev] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [LMPdev] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [LMPdev] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [LMPdev] SET  MULTI_USER 
GO
ALTER DATABASE [LMPdev] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [LMPdev] SET DB_CHAINING OFF 
GO
ALTER DATABASE [LMPdev] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [LMPdev] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [LMPdev] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'LMPfull', N'ON'
GO
ALTER DATABASE [LMPdev] SET QUERY_STORE = OFF
GO
USE [LMPdev]
GO
ALTER DATABASE SCOPED CONFIGURATION SET LEGACY_CARDINALITY_ESTIMATION = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION SET MAXDOP = 0;
GO
ALTER DATABASE SCOPED CONFIGURATION SET PARAMETER_SNIFFING = ON;
GO
ALTER DATABASE SCOPED CONFIGURATION SET QUERY_OPTIMIZER_HOTFIXES = OFF;
GO
USE [LMPdev]
GO
/****** Object:  User [LMP_USR]    Script Date: 4/15/2020 4:17:47 PM ******/
CREATE USER [LMP_USR] WITHOUT LOGIN WITH DEFAULT_SCHEMA=[dbo]
GO
ALTER ROLE [db_owner] ADD MEMBER [LMP_USR]
GO
/****** Object:  Schema [Component]    Script Date: 4/15/2020 4:17:47 PM ******/
CREATE SCHEMA [Component]
GO
/****** Object:  Schema [DFID\ma-robertson_admin]    Script Date: 4/15/2020 4:17:47 PM ******/
CREATE SCHEMA [DFID\ma-robertson_admin]
GO
/****** Object:  Schema [Location]    Script Date: 4/15/2020 4:17:47 PM ******/
CREATE SCHEMA [Location]
GO
/****** Object:  Schema [Project]    Script Date: 4/15/2020 4:17:47 PM ******/
CREATE SCHEMA [Project]
GO
/****** Object:  Schema [Results]    Script Date: 4/15/2020 4:17:47 PM ******/
CREATE SCHEMA [Results]
GO
/****** Object:  Schema [Shadow]    Script Date: 4/15/2020 4:17:47 PM ******/
CREATE SCHEMA [Shadow]
GO
/****** Object:  Schema [System]    Script Date: 4/15/2020 4:17:47 PM ******/
CREATE SCHEMA [System]
GO
/****** Object:  Schema [Tasks]    Script Date: 4/15/2020 4:17:47 PM ******/
CREATE SCHEMA [Tasks]
GO
/****** Object:  Schema [Workflow]    Script Date: 4/15/2020 4:17:47 PM ******/
CREATE SCHEMA [Workflow]
GO
/****** Object:  UserDefinedFunction [dbo].[FiscalYear]    Script Date: 4/15/2020 4:17:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[FiscalYear](
    @AsOf           DATETIME
)
RETURNS varchar(9)
AS
BEGIN

    DECLARE @Answer     INT

    -- You define what you want here (September being your changeover month)
    IF ( MONTH(@AsOf) < 4 )
        SET @Answer = YEAR(@AsOf) - 1
    ELSE
        SET @Answer = YEAR(@AsOf)


    RETURN convert(varchar(4),@Answer) + '-' + convert(varchar(4),(@Answer+1))

END



GO
/****** Object:  UserDefinedFunction [dbo].[LMP_GetEstimatedDuration]    Script Date: 4/15/2020 4:17:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[LMP_GetEstimatedDuration](@Start datetime, @End datetime)

RETURNS NVARCHAR(300)

AS
BEGIN
	DECLARE @duration AS VARCHAR(255) = ''
	if(@Start is not null and @End is not null)
	begin
		declare @totalDays int = datediff(DAY, @start, @end), @remainingDays int = datediff(DAY, @start, @end)
            if (@totalDays >= 365)
            BEGIN
				SET @duration += convert(varchar,@totalDays / 365)+ ' Year(s) '
				SET @remainingDays = @totalDays % 365;
            END
            else if (@totalDays >= 30)
            BEGIN
				SET @duration += convert(varchar,@totalDays / 30)+ ' Month(s) '
				SET @remainingDays = @totalDays % 30;
            END
            else if (@totalDays >= 7)
            BEGIN
				SET @duration += convert(varchar,@totalDays / 7)+ ' Week(s) '
				SET @remainingDays = @totalDays % 7;
            END
            else
            BEGIN
                SET @duration += convert(varchar,@totalDays) + ' Day(s) '
            END
	end

	RETURN @duration
END

GO
/****** Object:  UserDefinedFunction [dbo].[SplitString]    Script Date: 4/15/2020 4:17:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[SplitString]
(    
      @Input NVARCHAR(MAX),
      @Character CHAR(1)
)
RETURNS @Output TABLE (
      Item NVARCHAR(1000)
)
AS
BEGIN
      DECLARE @StartIndex INT, @EndIndex INT
 
      SET @StartIndex = 1
      IF SUBSTRING(@Input, LEN(@Input) - 1, LEN(@Input)) <> @Character
      BEGIN
            SET @Input = @Input + @Character
      END
 
      WHILE CHARINDEX(@Character, @Input) > 0
      BEGIN
            SET @EndIndex = CHARINDEX(@Character, @Input)
           
            INSERT INTO @Output(Item)
            SELECT SUBSTRING(@Input, @StartIndex, @EndIndex - 1)
           
            SET @Input = SUBSTRING(@Input, @EndIndex + 1, LEN(@Input))
      END
 
      RETURN
END
GO
/****** Object:  UserDefinedFunction [System].[RemoveNonStandardCharacters]    Script Date: 4/15/2020 4:17:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [System].[RemoveNonStandardCharacters](@String NVARCHAR(MAX))

RETURNS NVARCHAR(MAX)

AS
	BEGIN

		DECLARE @KeepValues AS VARCHAR(255)
		SET @KeepValues = '%[^-a-z0-9 .,&£$%*()+=#@:<>?\/]%'
		WHILE PATINDEX(@KeepValues, @String) > 0
			SET @String = STUFF(@String, PATINDEX(@KeepValues, @String), 1, '')

		RETURN @String
	END

GO
/****** Object:  Table [Project].[Performance]    Script Date: 4/15/2020 4:17:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Project].[Performance](
	[ProjectID] [varchar](6) NOT NULL,
	[ARRequired] [varchar](3) NULL,
	[ARExemptJustification] [varchar](1000) NULL,
	[ARDueDate] [datetime] NULL,
	[ARPromptDate] [datetime] NULL,
	[ARDefferal] [varchar](3) NULL,
	[PCRRequired] [varchar](3) NULL,
	[PCRExemptJustification] [varchar](1000) NULL,
	[PCRDueDate] [datetime] NULL,
	[PCRPrompt] [datetime] NULL,
	[PCRDefferal] [varchar](3) NULL,
	[PCRDefferalJustification] [varchar](1000) NULL,
	[PCRAuthorised] [varchar](3) NULL,
	[ARExcemptReason] [varchar](1000) NULL,
	[PCRExcemptReason] [varchar](1000) NULL,
	[DefferalTimeScale] [int] NULL,
	[DeferralReason] [varchar](1000) NULL,
	[Status] [varchar](6) NULL,
	[LastUpdated] [datetime] NULL,
	[UserID] [varchar](6) NULL,
PRIMARY KEY CLUSTERED 
(
	[ProjectID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [Project].[ProjectMaster]    Script Date: 4/15/2020 4:17:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Project].[ProjectMaster](
	[ProjectID] [varchar](6) NOT NULL,
	[Title] [varchar](max) NULL,
	[Description] [varchar](max) NULL,
	[BudgetCentreID] [varchar](5) NULL,
	[Stage] [varchar](5) NULL,
	[Status] [varchar](255) NULL,
	[LastUpdate] [datetime] NULL,
	[UserID] [varchar](6) NULL,
PRIMARY KEY CLUSTERED 
(
	[ProjectID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [Project].[Team]    Script Date: 4/15/2020 4:17:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Project].[Team](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ProjectID] [varchar](6) NOT NULL,
	[TeamID] [varchar](255) NOT NULL,
	[RoleID] [varchar](255) NOT NULL,
	[Status] [varchar](255) NULL,
	[StartDate] [datetime] NOT NULL,
	[EndDate] [datetime] NULL,
	[LastUpdated] [datetime] NOT NULL,
	[UserID] [varchar](6) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [Tasks].[vAMPAlerts]    Script Date: 4/15/2020 4:17:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE VIEW [Tasks].[vAMPAlerts]
AS

Select 
	P.ProjectID as ProjectID,
	'Annual Review is due on ' + CAST(DAY(Perf.ARDueDate) AS VARCHAR(2)) + ' ' + DATENAME(MONTH, Perf.ARDueDate) + ' ' + CAST(YEAR(Perf.ARDueDate) AS VARCHAR(4)) AS [Message],
	'/Project/Reviews/' + p.ProjectID as [Path],
	Team.TeamID as 'UserID',
	'3' as Severity
from 
	Project.ProjectMaster P 
	INNER JOIN Project.Performance Perf ON p.ProjectID = perf.ProjectID
	INNER JOIN Project.Team Team ON p.ProjectID = Team.ProjectID
Where
	Perf.ARPromptDate <= GetDate() and 
	Perf.ARRequired = 'Yes' and 
	Perf.ARDueDate > '1900 JAN 01 :00:00:00' and
	Perf.ARDueDate >= GETDATE() and
	Team.ProjectID = p.ProjectID and
	Team.Status = 'A'
UNION ALL
Select 
	P.ProjectID as ProjectID,
	'Annual Review is overdue' AS [Message],
	'/Project/Reviews/' + p.ProjectID as [Path],
	Team.TeamID as 'UserID',
	'1' as Severity
from 
	Project.ProjectMaster P 
	INNER JOIN Project.Performance Perf ON p.ProjectID = perf.ProjectID
	INNER JOIN Project.Team Team ON p.ProjectID = Team.ProjectID
Where
	Perf.ARRequired = 'Yes' and 
	Perf.ARDueDate > '1900 JAN 01 :00:00:00' and
	Perf.ARDueDate < GETDATE() and
	Team.ProjectID = p.ProjectID and
	Team.Status = 'A'
UNION ALL
Select 
	P.ProjectID as ProjectID,
	'PCR is due on ' + CAST(DAY(Perf.PCRDueDate) AS VARCHAR(2)) + ' ' + DATENAME(MONTH, Perf.PCRDueDate) + ' ' + CAST(YEAR(Perf.PCRDueDate) AS VARCHAR(4)) AS [Message],
	'/Project/Reviews/' + p.ProjectID as [Path],
	Team.TeamID as 'UserID',
	'3' as Severity
from 
	Project.ProjectMaster P 
	INNER JOIN Project.Performance Perf ON p.ProjectID = perf.ProjectID
	INNER JOIN Project.Team Team ON p.ProjectID = Team.ProjectID
Where
	Perf.PCRPrompt <= GetDate() and 
	Perf.PCRRequired = 'Yes' and 
	Perf.PCRDueDate > '1900 JAN 01 :00:00:00' and
	Perf.PCRDueDate >= GETDATE() and
	Team.ProjectID = p.ProjectID and
	Team.Status = 'A'
UNION ALL
Select 
	P.ProjectID as ProjectID,
	'Project Completion Review is overdue' AS [Message],
	'/Project/Reviews/' + p.ProjectID as [Path],
	Team.TeamID as 'UserID',
	'1' as Severity
from 
	Project.ProjectMaster P 
	INNER JOIN Project.Performance Perf ON p.ProjectID = perf.ProjectID
	INNER JOIN Project.Team Team ON p.ProjectID = Team.ProjectID
Where
	Perf.PCRRequired = 'Yes' and 
	Perf.PCRDueDate > '1900 JAN 01 :00:00:00' and
	Perf.PCRDueDate < GETDATE() and
	Team.ProjectID = p.ProjectID and
	Team.Status = 'A'



GO
/****** Object:  Table [Workflow].[WorkflowMaster]    Script Date: 4/15/2020 4:17:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Workflow].[WorkflowMaster](
	[WorkFlowID] [int] NOT NULL,
	[WorkFlowStepID] [int] NOT NULL,
	[TaskID] [int] NOT NULL,
	[StageID] [int] NOT NULL,
	[ProjectID] [varchar](6) NOT NULL,
	[ActionBy] [varchar](6) NULL,
	[ActionDate] [datetime] NULL,
	[Recipient] [varchar](6) NULL,
	[ActionComments] [varchar](500) NULL,
	[Status] [varchar](255) NULL,
	[LastUpdate] [datetime] NULL,
	[UserID] [varchar](6) NULL,
PRIMARY KEY CLUSTERED 
(
	[WorkFlowID] ASC,
	[WorkFlowStepID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [Workflow].[WorkflowTask]    Script Date: 4/15/2020 4:17:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Workflow].[WorkflowTask](
	[TaskID] [int] NOT NULL,
	[Description] [varchar](500) NOT NULL,
	[Status] [varchar](255) NULL,
	[LastUpdate] [datetime] NULL,
	[UserID] [varchar](6) NULL,
PRIMARY KEY CLUSTERED 
(
	[TaskID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [Project].[ReviewDeferral]    Script Date: 4/15/2020 4:17:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Project].[ReviewDeferral](
	[DeferralID] [int] IDENTITY(1,1) NOT NULL,
	[ProjectID] [varchar](6) NOT NULL,
	[ReviewID] [int] NOT NULL,
	[StageID] [varchar](5) NOT NULL,
	[DeferralTimescale] [varchar](3) NULL,
	[DeferralJustification] [varchar](500) NULL,
	[ApproverComment] [varchar](500) NULL,
	[Approver] [varchar](6) NULL,
	[Approved] [varchar](1) NULL,
	[Requester] [varchar](6) NULL,
	[PreviousReviewDate] [datetime] NULL,
	[LastUpdated] [datetime] NULL,
	[UpdatedBy] [varchar](10) NULL,
 CONSTRAINT [PK__ReviewDe__292D70C770D3A237] PRIMARY KEY CLUSTERED 
(
	[DeferralID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [Project].[ReviewExemption]    Script Date: 4/15/2020 4:17:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Project].[ReviewExemption](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ProjectID] [varchar](6) NOT NULL,
	[StageID] [varchar](5) NOT NULL,
	[ExemptionType] [varchar](50) NOT NULL,
	[ExemptionReason] [varchar](1000) NULL,
	[ApproverComment] [varchar](200) NULL,
	[Approver] [varchar](6) NULL,
	[Approved] [varchar](1) NULL,
	[SubmissionComment] [varchar](200) NULL,
	[Requester] [varchar](6) NULL,
	[LastUpdated] [datetime] NULL,
	[UpdatedBy] [varchar](10) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [Project].[ReviewMaster]    Script Date: 4/15/2020 4:17:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Project].[ReviewMaster](
	[ProjectID] [varchar](6) NOT NULL,
	[ReviewID] [int] NOT NULL,
	[ReviewType] [varchar](255) NULL,
	[ReviewDate] [datetime] NULL,
	[DeferralDate] [datetime] NULL,
	[RiskScore] [varchar](10) NULL,
	[Status] [varchar](6) NULL,
	[Approved] [varchar](1) NULL,
	[LastUpdated] [datetime] NULL,
	[UserID] [varchar](6) NULL,
	[Approver] [varchar](6) NULL,
	[Requester] [varchar](6) NULL,
	[StageID] [varchar](5) NULL,
	[SubmissionComment] [varchar](500) NULL,
	[ApproveComment] [varchar](500) NULL,
	[ReviewScore] [decimal](18, 1) NULL,
	[DueDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[ProjectID] ASC,
	[ReviewID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [Tasks].[vAMPTasks]    Script Date: 4/15/2020 4:17:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [Tasks].[vAMPTasks]                                                   
AS

SELECT P.Title,Z.* FROM 
(
Select 
       wf.ProjectID AS ProjectID, 
       wf.ActionBy AS Sender, 
       wf.Recipient AS Recipient, 
       wf.ActionDate AS ActionDate, 
       wf.ActionComments AS SenderComments, 
       CASE wt.Description
       WHEN 'Admin or Rapid Response' THEN 'Approve Admin or Rapid Response' ELSE wt.Description END AS TaskDescription,
       '/Workflow/Edit/' + wf.ProjectID + '/' + CAST(wf.TaskID AS varchar(2))  AS [Path]
from 
       Workflow.WorkflowMaster wf, 
       Workflow.WorkflowTask wt
Where
       wf.TaskID = wt.taskID and wf.status = 'A'
UNION ALL
Select
       ProjectID as ProjectID,
       UserID as Sender,
       Approver as Recipient,
       LastUpdated as ActionDate,
       SubmissionComment as SenderComments,
       'Approve ' + ReviewType as TaskDescription,
       '/Project/Reviews/' + ProjectID as [Path]
from 
       Project.ReviewMaster 
where 
       StageId = 1
UNION ALL
Select
       ProjectID as ProjectID,
       Requester as Sender,
       Approver as Recipient,
       LastUpdated as ActionDate,
       DeferralJustification as SenderComments,
       'Approve Annual Review Deferral' as TaskDescription,
       '/Project/Reviews/' + ProjectID as [Path]
from 
       Project.ReviewDeferral
where 
       StageId = 1
UNION ALL
Select
       ProjectID as ProjectID,
       Requester as Sender,
       Approver as Recipient,
       LastUpdated as ActionDate,
       SubmissionComment as SenderComments,
       'Approve ' + ExemptionType + ' Exemption' as TaskDescription,
       '/Project/Reviews/' + ProjectID as [Path]
from 
       Project.ReviewExemption
where 
       StageId = 1
)Z
INNER JOIN 
       Project.ProjectMaster P
ON 
       Z.ProjectID = P.ProjectID

GO
/****** Object:  View [Workflow].[V_ProjectClosure]    Script Date: 4/15/2020 4:17:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE VIEW [Workflow].[V_ProjectClosure]
WITH SCHEMABINDING
AS
WITH Step1 AS
(
SELECT 
	ProjectID,
	ReviewDate AS ClosureDate,
	Approver
FROM
	[Project].[ReviewMaster]
WHERE
	ReviewType = 'Project Completion Review'
AND
	approved = '1'
AND
	Approver IS NOT NULL

UNION ALL 

SELECT 
	ProjectID,
	ActionDate AS ClosureDate,
	ActionBy AS Approver
FROM
	[Workflow].[WorkflowMaster]
WHERE
	TaskID = '1'
AND
	StageID = '2'
AND
	Status = 'C'
),

Step2 AS
(
SELECT
	ProjectId ,
	ClosureDate ,
	Approver ,
	ROW_NUMBER() OVER(PARTITION BY ProjectId ORDER BY ClosureDate DESC) AS [Ranking]
FROM
	Step1
)

SELECT
	ProjectID,
	ClosureDate,
	Approver
FROM 
	Step2
WHERE
	[Ranking] = 1

GO
/****** Object:  Table [dbo].[TBL_Contracts]    Script Date: 4/15/2020 4:17:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TBL_Contracts](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PQId] [int] NOT NULL,
	[ContractorCIF] [varchar](100) NULL,
	[ContractorName] [varchar](500) NULL,
	[CGSId] [varchar](100) NULL,
	[ContractType] [varchar](100) NULL,
	[Description] [nvarchar](max) NULL,
	[TenderIssueDate] [datetime] NULL,
	[TenderLastDate] [datetime] NULL,
	[PACDate] [datetime] NULL,
	[DefectsLiabilityEndDate] [datetime] NULL,
	[FACDate] [datetime] NULL,
	[CompReportDate] [datetime] NULL,
	[AdvPmtGrntAmount] [numeric](18, 2) NULL,
	[AdvPmtGrntExpiry] [datetime] NULL,
	[PerBankGrntAmount] [numeric](18, 2) NULL,
	[PerBankGrntExpiry] [datetime] NULL,
	[OtherGrntAmount] [numeric](18, 2) NULL,
	[OtherGrntExpiry] [datetime] NULL,
	[PackageId] [nvarchar](max) NULL,
	[PackageName] [nvarchar](max) NULL,
	[PackageDisplayId] [nvarchar](max) NULL,
	[EstimateValue] [decimal](16, 6) NULL,
	[TypeOfPackage] [nvarchar](max) NULL,
	[LocAmount] [decimal](16, 6) NULL,
	[GovtAmount] [decimal](16, 6) NULL,
	[OtherAmount] [decimal](16, 6) NULL,
	[ContractStart] [datetime] NULL,
	[ContractEnd] [datetime] NULL,
	[GuaranteeNote] [varchar](max) NULL,
	[Note] [varchar](max) NULL,
	[ScheduledCompDate] [datetime] NULL,
	[ContractApprovalDate] [datetime] NULL,
	[RevisedCompletionDate] [datetime] NULL,
	[ActualCompletionDate] [datetime] NULL,
	[TerminalDateOfDisbursement] [datetime] NULL,
	[DateOfReceiptOfContractByEximBank] [datetime] NULL,
	[SigningDate] [datetime] NULL,
	[SignEffectiveDate] [datetime] NULL,
	[DurationYear] [int] NULL,
	[DurationMonth] [int] NULL,
	[DurationDay] [int] NULL,
 CONSTRAINT [PK__TBL_Cont__3214EC07C147D47E] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TBL_LocBalance]    Script Date: 4/15/2020 4:17:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TBL_LocBalance](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Date] [datetime] NULL,
	[ConfirmedBy] [varchar](max) NULL,
	[Period] [varchar](max) NULL,
	[LocId] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TBL_Country]    Script Date: 4/15/2020 4:17:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TBL_Country](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](300) NULL,
	[CIF] [varchar](30) NULL,
	[RegionId] [int] NOT NULL,
	[AddedOn] [datetime] NULL,
	[AddedBy] [int] NULL,
	[UpdatedBy] [int] NULL,
	[UpdatedOn] [datetime] NULL,
	[RedFlag] [varchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TBL_LOC]    Script Date: 4/15/2020 4:17:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TBL_LOC](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](300) NULL,
	[CountryId] [int] NOT NULL,
	[ApprovalBy] [int] NULL,
	[ApprovalDate] [datetime] NULL,
	[AmendmentNumber] [int] NULL,
	[OmNumber] [varchar](100) NULL,
	[TotalAmount] [money] NULL,
	[CurrencyCode] [varchar](3) NULL,
	[TerminalDate] [date] NULL,
	[MEAAppDate] [datetime] NULL,
	[MdAppDate] [datetime] NULL,
	[FileNumber] [varchar](30) NULL,
	[OfferLetterDate] [datetime] NULL,
	[Purpose] [varchar](max) NULL,
	[InterestRate] [float] NOT NULL,
	[CommitmentFee] [float] NOT NULL,
	[ManagementFee] [float] NOT NULL,
	[InterestEqualization] [varchar](max) NULL,
	[Tenure] [int] NULL,
	[Moratorium] [int] NULL,
	[IndianContribution] [float] NULL,
	[SpecialCondition] [varchar](max) NULL,
	[GOIDeedDate] [datetime] NULL,
	[LocAccountNo] [varchar](30) NULL,
	[CrtAccountNo] [varchar](30) NULL,
	[SigningDate] [datetime] NULL,
	[EffectiveDate] [datetime] NULL,
	[AgreementAmendmentDate] [datetime] NULL,
	[Type] [varchar](max) NULL,
	[Percentage] [float] NULL,
	[ApprovalType] [varchar](max) NULL,
	[LocNumber] [varchar](100) NULL,
	[Classification] [varchar](50) NULL,
	[DeaDate] [datetime] NULL,
	[VerificationNote] [varchar](max) NULL,
	[InterestType] [varchar](max) NULL,
	[MEA_Type] [varchar](max) NULL,
	[MEA_Percentage] [float] NULL,
	[DEA_Type] [varchar](max) NULL,
	[DEA_Percentage] [float] NULL,
 CONSTRAINT [PK__TBL_LOC__3214EC07F9FB91FC] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TBL_LOC_Project]    Script Date: 4/15/2020 4:17:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TBL_LOC_Project](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[LocId] [int] NOT NULL,
	[ProjectId] [int] NOT NULL,
	[Allocation] [money] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TBL_Projects]    Script Date: 4/15/2020 4:17:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TBL_Projects](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Code] [varchar](100) NULL,
	[Name] [varchar](max) NULL,
	[Description] [varchar](max) NULL,
	[Status] [int] NULL,
	[DPRDate] [datetime] NULL,
	[BaselineData] [varchar](max) NULL,
	[LocationAddress] [varchar](300) NULL,
	[LocationCoordinates] [varchar](50) NULL,
	[Stage] [int] NULL,
	[SubSector] [int] NULL,
	[PreQualification] [bit] NULL,
	[AuthorityName] [varchar](max) NULL,
	[Progress] [float] NULL,
	[FinancialProgress] [float] NULL,
	[ProjectValue] [decimal](18, 2) NOT NULL,
	[ProjectStart] [datetime] NULL,
	[ProjectEnd] [datetime] NULL,
	[CrOn] [datetime] NOT NULL,
	[ModOn] [datetime] NULL,
	[IsActive] [bit] NULL,
	[Sector] [varchar](200) NULL,
	[SubSectorName] [varchar](max) NULL,
	[Address] [varchar](max) NULL,
	[AddressLat] [decimal](18, 0) NULL,
	[AddressLon] [decimal](18, 0) NULL,
	[DisbursedAmount] [decimal](18, 2) NULL,
	[Note] [varchar](max) NULL,
 CONSTRAINT [PK__TBL_Proj__3214EC07C64F4326] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TBL_Projects_PQ]    Script Date: 4/15/2020 4:17:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TBL_Projects_PQ](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](300) NULL,
	[Description] [varchar](max) NULL,
	[PQRefNumber] [varchar](100) NULL,
	[ApplicationStart] [datetime] NULL,
	[ApplicationEnd] [datetime] NULL,
	[ProjectId] [int] NOT NULL,
	[addendum_refno] [varchar](200) NULL,
	[pq_status] [int] NULL,
	[PqNo] [nvarchar](max) NULL,
	[PqId] [nvarchar](max) NULL,
	[Ref] [nvarchar](max) NULL,
	[Title] [nvarchar](max) NULL,
	[Country] [nvarchar](max) NULL,
	[LocNumber] [nvarchar](max) NULL,
	[LocAmount] [decimal](16, 6) NULL,
	[NoOfPackage] [int] NULL,
	[ProjectCost] [decimal](18, 2) NULL,
	[Category] [varchar](10) NULL,
	[PqDocPublishedOn] [datetime] NULL,
	[LastSubmissionOn] [datetime] NULL,
	[Status] [varchar](max) NULL,
	[ProjectPQSigninDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Finacle_LocFinancials]    Script Date: 4/15/2020 4:17:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Finacle_LocFinancials](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[LimitPrefix] [varchar](100) NULL,
	[FORACID] [varchar](100) NULL,
	[AccountName] [varchar](300) NULL,
	[LoanOutstanding] [decimal](24, 6) NULL,
	[TotalDisbursed] [decimal](24, 6) NULL,
	[PrincipalDemand] [decimal](24, 6) NULL,
	[PrincipalCollection] [decimal](24, 6) NULL,
	[PrincipalOverdue] [decimal](24, 6) NULL,
	[InterestDemand] [decimal](24, 6) NULL,
	[InterestCollection] [decimal](24, 6) NULL,
	[InterestOverdue] [decimal](24, 6) NULL,
	[SyncDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TBL_Regions]    Script Date: 4/15/2020 4:17:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TBL_Regions](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](max) NULL,
	[AddedOn] [datetime] NOT NULL,
	[UpdatedOn] [datetime] NULL,
	[UpdatedBy] [int] NULL,
	[AddedBy] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Finacle_PrincipalDue]    Script Date: 4/15/2020 4:17:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Finacle_PrincipalDue](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[AccountId] [varchar](200) NULL,
	[DemandType] [varchar](100) NULL,
	[DueDate] [datetime] NULL,
	[Frequency] [varchar](100) NULL,
	[DueAmount] [decimal](16, 6) NULL,
	[RunDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Finacle_InterestDue]    Script Date: 4/15/2020 4:17:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Finacle_InterestDue](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[AccountId] [varchar](200) NULL,
	[LastDate] [datetime] NULL,
	[NextDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Finacle_Contract_Transanctions]    Script Date: 4/15/2020 4:17:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Finacle_Contract_Transanctions](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[AccountId] [varchar](200) NULL,
	[CIF] [varchar](200) NULL,
	[TranDate] [datetime] NULL,
	[TranAmount] [decimal](16, 6) NULL,
	[Particulars] [varchar](max) NULL,
	[Currency] [varchar](100) NULL,
	[SanctionedAmount] [decimal](16, 6) NULL,
	[CummulativeDebit] [decimal](16, 6) NULL,
	[CummulativeCredit] [decimal](16, 6) NULL,
	[TranId] [varchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Finacle_RepaymentSchedule]    Script Date: 4/15/2020 4:17:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Finacle_RepaymentSchedule](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FORACID] [varchar](max) NULL,
	[LimitPrefix] [varchar](max) NULL,
	[AccountName] [varchar](max) NULL,
	[FlowStart] [datetime] NULL,
	[FlowId] [varchar](max) NULL,
	[FlowAmount] [decimal](16, 6) NULL,
	[Currency] [varchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Finacle_Disbursement]    Script Date: 4/15/2020 4:17:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Finacle_Disbursement](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ACID] [varchar](max) NULL,
	[LimitPrefix] [varchar](max) NULL,
	[LimitB2KID] [varchar](max) NULL,
	[FORACID] [varchar](max) NULL,
	[AccountName] [varchar](max) NULL,
	[SanctionLimit] [decimal](16, 6) NULL,
	[DisbAmount] [decimal](16, 6) NULL,
	[CurrencyCode] [varchar](max) NULL,
	[DisDate] [datetime] NULL,
	[DisbSerialNo] [varchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  View [dbo].[report_All]    Script Date: 4/15/2020 4:17:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE view [dbo].[report_All] as 

select 

--LOC.Id [LOC_Id],
LOC.Name [LOC_Name],
--LOC.CountryId [LOC_CountryId],
--LOC.ApprovalBy [LOC_ApprovalBy],
--LOC.ApprovalDate [LOC_ApprovalDate],
--LOC.AmendmentNumber [LOC_AmendmentNumber],
LOC.OmNumber [LOC_OmNumber],
LOC.TotalAmount [LOC_AllocatedAmount],
--LOC.CurrencyCode [LOC_CurrencyCode],
LOC.TerminalDate [LOC_TerminalDate],
LOC.MEAAppDate [LOC_MEAAppDate],
LOC.MdAppDate [LOC_MdAppDate],
--LOC.FileNumber [LOC_FileNumber],
LOC.OfferLetterDate [LOC_OfferLetterDate],
LOC.Purpose [LOC_Purpose],
LOC.InterestRate [LOC_InterestRate],
LOC.CommitmentFee [LOC_CommitmentFee],
LOC.ManagementFee [LOC_ManagementFee],
--LOC.InterestEqualization [LOC_InterestEqualization],
LOC.Tenure [LOC_Tenure],
LOC.Moratorium [LOC_Moratorium],
LOC.IndianContribution [LOC_IndianContribution],
LOC.SpecialCondition [LOC_SpecialCondition],
--LOC.GOIDeedDate [LOC_GOIDeedDate],
LOC.LocAccountNo [LOC_LocAccountNo],
--LOC.CrtAccountNo [LOC_CrtAccountNo],
LOC.SigningDate [LOC_SigningDate],
--LOC.EffectiveDate [LOC_EffectiveDate],
LOC.AgreementAmendmentDate [LOC_AgreementAmendmentDate],
--LOC.Type [LOC_Type],
--LOC.Percentage [LOC_Percentage],
--LOC.ApprovalType [LOC_ApprovalType],
LOC.LocNumber [LOC_LocNumber],
LOC.Classification [LOC_Classification],
LOC.DeaDate [LOC_DeaDate],
LOC.VerificationNote [LOC_VerificationNote],
LOC.InterestType [LOC_InterestType],
LOC.MEA_Type [LOC_MEA_Type],
LOC.MEA_Percentage [LOC_MEA_Percentage],
LOC.DEA_Type [LOC_DEA_Type],
LOC.DEA_Percentage [LOC_DEA_Percentage],

 isnull((select MAX(SanctionLimit) from Finacle_Disbursement where FORACID = LOC.LocAccountNo),0) [LOC_Sanctioned Amount]
	  ,isnull((select SUM(DisbAmount) from Finacle_Disbursement where FORACID = LOC.LocAccountNo),0) [LOC_Disbursed Amount]
	  ,isnull((select MIN(DisDate) from Finacle_Disbursement where FORACID = LOC.LocAccountNo),0) [LOC_First Disbursement]
	  ,(select count(Id) from TBL_LOC_Project where LocId = LOC.Id) [LOC_ProjectCount]
	  ,isnull((select SUM(PrincipalCollection) from  Finacle_LocFinancials where FORACID = LOC.LocAccountNo),0) [LOC_Amount Repaid]
	  ,isnull((select SUM(LoanOutstanding) from  Finacle_LocFinancials where FORACID = LOC.LocAccountNo),0) [LOC_Amount Outstanding]
	  ,isnull((select SUM(PrincipalOverdue) from  Finacle_LocFinancials where FORACID = LOC.LocAccountNo),0) [LOC_Principal Overdue]
	  ,isnull((select SUM(InterestOverdue) from  Finacle_LocFinancials where FORACID = LOC.LocAccountNo),0) [LOC_Interest Overdue]
	  ,isnull((select min(FlowStart) from Finacle_RepaymentSchedule where FlowId = 'PRDEM' and FORACID = LOC.LocAccountNo),'')[LOC_Principal Repayment Start]
	  ,isnull((select max(FlowStart) from Finacle_RepaymentSchedule where FlowId = 'PRDEM' and FORACID = LOC.LocAccountNo),'')[LOC_Principal Repayment End]
	  ,(select convert(varchar(6),NextDate, 113) + ', '+ convert(varchar(6),LastDate, 113) from Finacle_InterestDue where AccountId = LOC.LocAccountNo) [LOC_Interest Due Dates]
	  ,isnull((
			  SELECT 
			  --[AccountId],
			  STUFF((
			  SELECT top 2 ', ' + convert(varchar(6),DueDate, 113)
				FROM Finacle_PrincipalDue 
				WHERE DemandType = 'PRDEM' and AccountId = LOC.LocAccountNo and DueDate > getdate() order by DueDate
				FOR XML PATH(''),TYPE).value('(./text())[1]','VARCHAR(MAX)')
			  ,1,2,'') AS NameValues
			FROM Finacle_PrincipalDue Results
			where AccountId = LOC.LocAccountNo
			GROUP BY AccountId),'') [LOC_Principal Due Dates]
				, BAL.[Date] [LOC_BalanceConfirmationDate]
	, BAL.[ConfirmedBy] [LOC_BalanceConfirmationBy]
	, BAL.[Period] [LOC_BalanceConfirmationPeriod]
	, TR.Name [LOC_Region]
	, CN.Name [LOC_Country],

LP.Allocation [Project_LOCAllocationAllocation],


--P.Id [Project_Id],
P.Code [Project_Code],
P.Name [Project_Name],
P.Description [Project_Description],
--P.Status [Project_Status],
dbo.LMP_GetEstimatedDuration(P.[ProjectStart], P.[ProjectEnd]) [Project_EstimatedDuration],
--P.DPRDate [Project_DPRDate],
--P.BaselineData [Project_BaselineData],
--P.LocationAddress [Project_LocationAddress],
--P.LocationCoordinates [Project_LocationCoordinates],
--P.Stage [Project_Stage],
--P.SubSector [Project_SubSector],
--P.PreQualification [Project_PreQualification],
--P.AuthorityName [Project_AuthorityName],
P.Progress [Project_Progress],
P.FinancialProgress [Project_FinancialProgress],
P.ProjectValue [Project_ProjectValue],
P.ProjectStart [Project_ProjectStart],
P.ProjectEnd [Project_ProjectEnd],
--P.CrOn [Project_CrOn],
--P.ModOn [Project_ModOn],
--P.IsActive [Project_IsActive],
P.Sector [Project_Sector],
P.SubSectorName [Project_SubSectorName],
P.Address [Project_Address],
--P.AddressLat [Project_AddressLat],
--P.AddressLon [Project_AddressLon],
P.DisbursedAmount [Project_DisbursedAmount],
P.Note [Project_Note],


--PQ.Id [PQ_Id],
PQ.Name [PQ_Name],
PQ.Description [PQ_Description],
PQ.PQRefNumber [PQ_PQRefNumber],
PQ.ApplicationStart [PQ_ApplicationStart],
PQ.ApplicationEnd [PQ_ApplicationEnd],
--PQ.ProjectId [PQ_ProjectId],
PQ.addendum_refno [PQ_addendum_refno],
--PQ.pq_status [PQ_pq_status],
--PQ.PqNo [PQ_PqNo],
--PQ.PqId [PQ_PqId],
PQ.Ref [PQ_Ref],
PQ.Title [PQ_Title],
--PQ.Country [PQ_Country],
--PQ.LocNumber [PQ_LocNumber],
--PQ.LocAmount [PQ_LocAmount],
--PQ.NoOfPackage [PQ_NoOfPackage],
--PQ.ProjectCost [PQ_ProjectCost],
PQ.Category [PQ_Category],
PQ.PqDocPublishedOn [PQ_PqDocPublishedOn],
PQ.LastSubmissionOn [PQ_LastSubmissionOn],
PQ.Status [PQ_Status],

--CON.Id [Contract_Id],
--CON.PQId [Contract_PQId],
CON.ContractorCIF [Contract_ContractorCIF],
CON.ContractorName [Contract_ContractorName],
CON.CGSId [Contract_CGSId],
CON.ContractType [Contract_ContractType],
CON.Description [Contract_Description],
CON.TenderIssueDate [Contract_TenderIssueDate],
CON.TenderLastDate [Contract_TenderLastDate],
CON.PACDate [Contract_PACDate],
CON.DefectsLiabilityEndDate [Contract_DefectsLiabilityEndDate],
CON.FACDate [Contract_FACDate],
CON.CompReportDate [Contract_CompReportDate],
CON.AdvPmtGrntAmount [Contract_AdvPmtGrntAmount],
CON.AdvPmtGrntExpiry [Contract_AdvPmtGrntExpiry],
CON.PerBankGrntAmount [Contract_PerBankGrntAmount],
CON.PerBankGrntExpiry [Contract_PerBankGrntExpiry],
CON.OtherGrntAmount [Contract_OtherGrntAmount],
CON.OtherGrntExpiry [Contract_OtherGrntExpiry],
CON.PackageId [Contract_PackageId],
CON.PackageName [Contract_PackageName],
--CON.PackageDisplayId [Contract_PackageDisplayId],
CON.EstimateValue [Contract_EstimateValue],
CON.TypeOfPackage [Contract_TypeOfPackage],
CON.LocAmount [Contract_LocAmount],
CON.GovtAmount [Contract_GovtAmount],
CON.OtherAmount [Contract_OtherAmount],
CON.ContractStart [Contract_ContractStart],
CON.ContractEnd [Contract_ContractEnd],
CON.GuaranteeNote [Contract_GuaranteeNote],
CON.Note [Contract_Note],
CON.ScheduledCompDate [Contract_ScheduledCompDate],
CON.ContractApprovalDate [Contract_ContractApprovalDate],
CON.RevisedCompletionDate [Contract_RevisedCompletionDate],
CON.ActualCompletionDate [Contract_ActualCompletionDate],
CON.TerminalDateOfDisbursement [Contract_TerminalDateOfDisbursement],
CON.DateOfReceiptOfContractByEximBank [Contract_DateOfReceiptOfContractByEximBank],
--CON.SigningDate [Contract_SigningDate],
CON.SignEffectiveDate [Contract_SignEffectiveDate]


,(select MAX(SanctionedAmount) from Finacle_Contract_Transanctions where Particulars not like '%reversal%' and AccountId = CON.CGSId) [Contract_Sanctioned Amount]
,(select MAX(CummulativeDebit) from Finacle_Contract_Transanctions where Particulars not like '%reversal%' and AccountId = CON.CGSId) [Contract_Disbursed Amount]

from TBL_LOC LOC inner join TBL_LOC_Project LP on LOC.Id = LP.LocId
inner join TBL_Projects P on LP.ProjectId = P.Id
inner join TBL_Projects_PQ PQ on P.Id = PQ.ProjectId
inner join TBL_Contracts CON on CON.PQId = PQ.Id

inner join TBL_Country CN on LOC.CountryId = CN.Id inner join TBL_Regions TR on CN.RegionId = TR.Id
left join TBL_LocBalance BAL on LOC.Id = BAL.LocId

GO
/****** Object:  View [dbo].[report_LOC]    Script Date: 4/15/2020 4:17:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE view [dbo].[report_LOC] as 

SELECT --TOP (1000) 
		--LOC.[Id]
      LOC.[Name]
      --,[CountryId]
	  ,C.[Name] [Country]
	  ,TR.[Name] [Region]
      --,[ApprovalBy]
      --,[ApprovalDate]
      --,[AmendmentNumber]
      ,[OmNumber]
      ,[TotalAmount] AmountAllocated
	  
      --,[CurrencyCode]
      ,[TerminalDate]
      ,[MEAAppDate]
	  , [EffectiveDate]
      ,[MdAppDate] 
      --,[FileNumber]
      ,[OfferLetterDate]
      ,[Purpose]
      ,[InterestRate]
      ,[CommitmentFee]
      ,[ManagementFee]
      --,[InterestEqualization]
      ,[Tenure] [Tenor]
      ,[Moratorium]
      ,[IndianContribution]
      ,[SpecialCondition]
      --,[GOIDeedDate]
      ,[LocAccountNo]
      --,[CrtAccountNo]
      ,[SigningDate]
      --,[EffectiveDate]
      ,[AgreementAmendmentDate]
      --,[Type] [Equalization Interest Type]
      --,[Percentage] [Equalization Interest Rate]
      --,[ApprovalType] [ApprovalFrom]
	  ,[MEA_Type]
	  ,[MEA_Percentage]
	  ,[DEA_Type]
	  ,[DEA_Percentage]
      ,[LocNumber]
      ,[Classification]
      ,[DeaDate] [OM Date]
      ,[VerificationNote] [LOC Status]
      ,[InterestType] [Interest Type]
	  ,isnull((select MAX(SanctionLimit) from Finacle_Disbursement where FORACID = LOC.LocAccountNo),0) [Sanctioned Amount]
	  ,isnull((select SUM(DisbAmount) from Finacle_Disbursement where FORACID = LOC.LocAccountNo),0) [Disbursed Amount]
	  ,isnull((select MIN(DisDate) from Finacle_Disbursement where FORACID = LOC.LocAccountNo),0) [First Disbursement]
	  ,(select count(Id) from TBL_LOC_Project where LocId = LOC.Id) [ProjectCount]
	  ,isnull((select SUM(PrincipalCollection) from  Finacle_LocFinancials where FORACID = LOC.LocAccountNo),0) [Amount Repaid]
	  ,isnull((select SUM(LoanOutstanding) from  Finacle_LocFinancials where FORACID = LOC.LocAccountNo),0) [Amount Outstanding]
	  ,isnull((select SUM(PrincipalOverdue) from  Finacle_LocFinancials where FORACID = LOC.LocAccountNo),0) [Principal Overdue]
	  ,isnull((select SUM(InterestOverdue) from  Finacle_LocFinancials where FORACID = LOC.LocAccountNo),0) [Interest Overdue]
	  ,isnull((select min(FlowStart) from Finacle_RepaymentSchedule where FlowId = 'PRDEM' and FORACID = LOC.LocAccountNo),'')[Principal Repayment Start]
	  ,isnull((select max(FlowStart) from Finacle_RepaymentSchedule where FlowId = 'PRDEM' and FORACID = LOC.LocAccountNo),'')[Principal Repayment End]
	  ,(select convert(varchar(6),NextDate, 113) + ', '+ convert(varchar(6),LastDate, 113) from Finacle_InterestDue where AccountId = LOC.LocAccountNo) [Interest Due Dates]
	  ,isnull((
			  SELECT 
			  --[AccountId],
			  STUFF((
			  SELECT top 2 ', ' + convert(varchar(6),DueDate, 113)
				FROM Finacle_PrincipalDue 
				WHERE DemandType = 'PRDEM' and AccountId = LOC.LocAccountNo and DueDate > getdate() order by DueDate
				FOR XML PATH(''),TYPE).value('(./text())[1]','VARCHAR(MAX)')
			  ,1,2,'') AS NameValues
			FROM Finacle_PrincipalDue Results
			where AccountId = LOC.LocAccountNo
			GROUP BY AccountId),'') [Principal Due Dates]
	, BAL.[Date] [BalanceConfirmationDate]
	, BAL.[ConfirmedBy] [BalanceConfirmationBy]
	, BAL.[Period] [BalanceConfirmationPeriod]
  FROM [TBL_LOC] LOC inner join TBL_Country C on LOC.CountryId = C.Id inner join TBL_Regions TR on C.RegionId = TR.Id
  left join TBL_LocBalance BAL on LOC.Id = BAL.LocId


GO
/****** Object:  Table [dbo].[Tbl_Applicants]    Script Date: 4/15/2020 4:17:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tbl_Applicants](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Status] [int] NULL,
	[Name] [varchar](255) NULL,
	[Organization] [varchar](255) NULL,
	[SupplierId] [varchar](255) NULL,
	[PqId] [int] NULL,
	[ApplicantNo] [varchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[report_PQDetails]    Script Date: 4/15/2020 4:17:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




CREATE view [dbo].[report_PQDetails] as

select 
-- PQ.Id [PQ_Id]
 PQ.Name [PQ_Name]
, PQ.Description [PQ_Description]
, PQ.PQRefNumber [PQ_PQRefNumber]
, PQ.ApplicationStart [PQ_ApplicationStart]
, PQ.ApplicationEnd [PQ_ApplicationEnd]
--, PQ.ProjectId [PQ_ProjectId]
, PQ.addendum_refno [PQ_addendum_refno]
--, PQ.pq_status [PQ_pq_status]
, PQ.PqNo [PQ_PqNo]
--, PQ.PqId [PQ_PqId]
--, PQ.Ref [PQ_Ref]
--, PQ.Title [PQ_Title]
--, PQ.Country [PQ_Country]
--, PQ.LocNumber [PQ_LocNumber]
, PQ.LocAmount [PQ_LocAmount]
, PQ.NoOfPackage [PQ_NoOfPackage]
--, PQ.ProjectCost [PQ_ProjectCost]
, PQ.Category [PQ_Category]
--, PQ.PqDocPublishedOn [PQ_PqDocPublishedOn]
, PQ.LastSubmissionOn [PQ_LastSubmissionOn]
, PQ.Status [PQ_Status]
--, P.Id [Project_Id]
, P.Code [Project_Code]
, P.Name [Project_Name]
, P.Description [Project_Description]
--, P.Status [Project_Status]
--, P.DPRDate [Project_DPRDate]
--, P.BaselineData [Project_BaselineData]
, P.LocationAddress [Project_LocationAddress]
--, P.LocationCoordinates [Project_LocationCoordinates]
--, P.Stage [Project_Stage]
--, P.SubSector [Project_SubSector]
--, P.PreQualification [Project_PreQualification]
--, P.AuthorityName [Project_AuthorityName]
--, P.Progress [Project_Progress]
, P.FinancialProgress [Project_FinancialProgress]
, P.ProjectValue [Project_ProjectValue]
, P.ProjectStart [Project_ProjectStart]
, P.ProjectEnd [Project_ProjectEnd]
--, P.CrOn [Project_CrOn]
--, P.ModOn [Project_ModOn]
--, P.IsActive [Project_IsActive]
, P.Sector [Project_Sector]
, P.SubSectorName [Project_SubSectorName]
, P.Address [Project_Address]
--, P.AddressLat [Project_AddressLat]
--, P.AddressLon [Project_AddressLon]
, P.DisbursedAmount [Project_DisbursedAmount]
, P.Note [Project_Note]
--, LP.Id [Allocation_Id]
--, LP.LocId [Allocation_LocId]
--, LP.ProjectId [Allocation_ProjectId]
, LP.Allocation [Allocation_Allocation]
,(select [Name] from TBL_Country where id = (select CountryId from TBL_LOC where Id = LP.LocId)) LOC_Country
, AP.Name [Applicant_Name]
, AP.Status [Applicant_Status]
, AP.Organization [Applicant_Org]
, AP.ApplicantNo [ApplicantNo]

from TBL_Projects_PQ PQ inner join TBL_Projects P on PQ.ProjectId = P.Id left join TBL_LOC_Project LP on P.Id = LP.ProjectId
left join Tbl_Applicants AP on PQ.Id = AP.PqId




GO
/****** Object:  Table [dbo].[TBL_Status]    Script Date: 4/15/2020 4:17:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TBL_Status](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[report_Project]    Script Date: 4/15/2020 4:17:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE view [dbo].[report_Project] as

SELECT --TOP (1000) --[Id]
      [Code]
      ,PRO.[Name] [ProjectName]
      ,PRO.[Description]
      ,TS.[Name] [Status]
      --,[DPRDate]
      --,[BaselineData]
      --,[LocationAddress]
      --,[LocationCoordinates]
      --,[Stage]
      --,[SubSector]
      --,[PreQualification]
      --,[AuthorityName]
      ,[Progress]
      ,[FinancialProgress]
      ,[ProjectValue]
      ,[ProjectStart]
      ,[ProjectEnd]
	  , dbo.LMP_GetEstimatedDuration([ProjectStart], [ProjectEnd]) [EstimatedDuration]
      --,[CrOn]
      --,[ModOn]
      --,[IsActive]
      ,[Sector]
      ,[SubSectorName]
      ,[Address]
      --,[AddressLat]
      --,[AddressLon]
      ,[DisbursedAmount]
	  ,(select count(Id) from TBL_LOC_Project where ProjectId = PRO.Id) [LOCCount]
	  ,PQ.PqNo
	  ,PQ.PqId
	  ,PQ.Ref [PQ Desc]
	  ,PQ.LocAmount [PQ Loc Amount]
	  ,PQ.NoOfPackage [PQ Package Count]
	  --,PQ.*
  FROM [TBL_Projects] PRO left join TBL_Status TS on PRO.[Status] = TS.Id
  left join TBL_Projects_PQ PQ on PRO.Id = PQ.ProjectId
  where IsActive = 1


GO
/****** Object:  Table [dbo].[TBL_ContractLC]    Script Date: 4/15/2020 4:17:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TBL_ContractLC](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ContractId] [int] NOT NULL,
	[LCNumber] [varchar](max) NULL,
	[Beneficiary] [varchar](max) NULL,
	[Amount] [decimal](16, 6) NULL,
	[OpeningDate] [datetime] NULL,
	[IssuingBank] [varchar](max) NULL,
	[LastDateofShipment] [datetime] NULL,
	[ExpiryDate] [datetime] NULL,
	[TransferableAmount] [decimal](16, 6) NULL,
	[LCNote] [varchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  View [dbo].[report_Contract]    Script Date: 4/15/2020 4:17:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE view [dbo].[report_Contract] as

SELECT --[Id]
      --,[PQId]
	  [PackageId] [Contract Number]
	  ,PQ.PqNo
      ,[ContractorCIF]
      ,[ContractorName]
      ,[CGSId] [BCD Account]
      ,[ContractType]
      ,CON.[Description]
      ,[TenderIssueDate]
      ,[TenderLastDate]
      ,[PACDate]
      ,[DefectsLiabilityEndDate]
      ,[FACDate]
      ,[CompReportDate] [DateofCompletionReport]
      ,[AdvPmtGrntAmount]
      ,[AdvPmtGrntExpiry]
      ,[PerBankGrntAmount]
      ,[PerBankGrntExpiry]
      ,[OtherGrntAmount]
      ,[OtherGrntExpiry]
      ,[PackageName] [Contract Name]
      ,[PackageDisplayId] [PQ Package No]
      ,[EstimateValue]
      ,[TypeOfPackage]
      ,CON.[LocAmount]
      ,[GovtAmount]
      ,[OtherAmount]
      ,[GuaranteeNote]
      ,[ContractStart]
      ,[ContractEnd] [ActualCommercialOpsDate]
	  ,[ScheduledCompDate]
	  ,[RevisedCompletionDate]
	  ,(select MAX(SanctionedAmount) from Finacle_Contract_Transanctions where Particulars not like '%reversal%' and AccountId = CON.CGSId) [Sanctioned Amount]
	  ,(select MAX(CummulativeDebit) from Finacle_Contract_Transanctions where Particulars not like '%reversal%' and AccountId = CON.CGSId) [Disbursed Amount]
	  ,LC.LCNumber , LC.Beneficiary, LC.Amount LCAmount, LC.ExpiryDate LCExpiryDate
	  
	  FROM [TBL_Contracts] CON inner join TBL_Projects_PQ PQ on PQ.Id = CON.PQId
	  left join TBL_ContractLC LC on CON.Id = LC.ContractId

	  --where CON.Id in (select ContractId from TBL_ContractLC group by ContractId having count(ContractId) > 1)
	  --order by [Contract Number]
	  --select * from [TBL_Contracts]

GO
/****** Object:  Table [Component].[ComponentDates]    Script Date: 4/15/2020 4:17:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Component].[ComponentDates](
	[ComponentID] [varchar](10) NOT NULL,
	[StartDate] [datetime] NULL,
	[EndDate] [datetime] NULL,
	[OperationalStartDate] [datetime] NULL,
	[OperationalEndDate] [datetime] NULL,
	[LastUpdate] [datetime] NULL,
	[UserID] [varchar](6) NULL,
PRIMARY KEY CLUSTERED 
(
	[ComponentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [Component].[ComponentMaster]    Script Date: 4/15/2020 4:17:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Component].[ComponentMaster](
	[ComponentID] [varchar](10) NOT NULL,
	[ComponentDescription] [varchar](max) NULL,
	[BudgetCentreID] [varchar](5) NULL,
	[ProjectID] [varchar](6) NULL,
	[AdminApprover] [varchar](7) NULL,
	[FundingMechanism] [varchar](255) NULL,
	[OperationalStatus] [varchar](255) NULL,
	[BenefittingCountry] [varchar](2) NULL,
	[LastUpdate] [datetime] NULL,
	[UserID] [varchar](6) NULL,
	[Approved] [varchar](5) NULL,
	[FundingArrangementValue] [varchar](20) NULL,
	[PartnerOrganisationValue] [varchar](20) NULL,
PRIMARY KEY CLUSTERED 
(
	[ComponentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [Component].[DeliveryChain]    Script Date: 4/15/2020 4:17:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Component].[DeliveryChain](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ComponentID] [varchar](10) NULL,
	[ChainID] [varchar](50) NOT NULL,
	[ParentID] [int] NOT NULL,
	[ParentType] [varchar](1) NULL,
	[ChildID] [int] NOT NULL,
	[ChildType] [varchar](1) NULL,
	[LastUpdate] [datetime] NULL,
	[UserID] [varchar](255) NULL,
	[Status] [varchar](1) NULL,
	[ParentNodeID] [int] NULL,
 CONSTRAINT [PK_DeliveryChain] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [Component].[ImplementingOrganisation]    Script Date: 4/15/2020 4:17:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Component].[ImplementingOrganisation](
	[ComponentID] [varchar](10) NOT NULL,
	[LineNo] [int] NOT NULL,
	[ImplementingOrganisation] [varchar](255) NULL,
	[LastUpdate] [datetime] NULL,
	[UserID] [varchar](6) NULL,
PRIMARY KEY CLUSTERED 
(
	[ComponentID] ASC,
	[LineNo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [Component].[InputSectorCodes]    Script Date: 4/15/2020 4:17:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Component].[InputSectorCodes](
	[ComponentID] [varchar](10) NOT NULL,
	[LineNo] [int] NOT NULL,
	[InputSectorCode] [varchar](7) NULL,
	[Percentage] [int] NULL,
	[LastUpdate] [datetime] NULL,
	[UserID] [varchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[ComponentID] ASC,
	[LineNo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [Component].[Markers]    Script Date: 4/15/2020 4:17:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Component].[Markers](
	[ComponentID] [varchar](10) NOT NULL,
	[PBA] [varchar](3) NOT NULL,
	[SWAP] [varchar](3) NOT NULL,
	[Status] [varchar](1) NULL,
	[LastUpdate] [datetime] NULL,
	[UserID] [varchar](6) NULL,
PRIMARY KEY CLUSTERED 
(
	[ComponentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [Component].[PartnerMaster]    Script Date: 4/15/2020 4:17:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Component].[PartnerMaster](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[PartnerID] [int] NOT NULL,
	[PartnerName] [varchar](255) NOT NULL,
	[Status] [varchar](1) NOT NULL,
	[LastUpdated] [datetime] NOT NULL,
	[UserID] [varchar](6) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [Component].[zComponentMaster_PreCR1515045]    Script Date: 4/15/2020 4:17:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Component].[zComponentMaster_PreCR1515045](
	[ComponentID] [varchar](10) NOT NULL,
	[ComponentDescription] [varchar](max) NULL,
	[BudgetCentreID] [varchar](5) NULL,
	[ProjectID] [varchar](6) NULL,
	[AdminApprover] [varchar](7) NULL,
	[FundingMechanism] [varchar](255) NULL,
	[OperationalStatus] [varchar](255) NULL,
	[BenefittingCountry] [varchar](2) NULL,
	[LastUpdate] [datetime] NULL,
	[UserID] [varchar](6) NULL,
	[Approved] [varchar](5) NULL,
	[FundingArrangementValue] [varchar](20) NULL,
	[PartnerOrganisationValue] [varchar](20) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [Component].[zDeliveryChain_PreCR1292793]    Script Date: 4/15/2020 4:17:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Component].[zDeliveryChain_PreCR1292793](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ComponentID] [varchar](10) NULL,
	[ChainID] [int] NOT NULL,
	[ParentID] [int] NOT NULL,
	[ParentType] [varchar](1) NULL,
	[ChildID] [int] NOT NULL,
	[ChildType] [varchar](1) NULL,
	[LastUpdate] [datetime] NULL,
	[UserID] [varchar](255) NULL,
	[Status] [varchar](1) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BobEProcure_Packages]    Script Date: 4/15/2020 4:17:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BobEProcure_Packages](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PQId] [varchar](max) NULL,
	[Ref] [varchar](max) NULL,
	[Title] [varchar](max) NULL,
	[Type] [varchar](max) NULL,
	[Country] [varchar](max) NULL,
	[Approver] [varchar](max) NULL,
	[Status] [int] NULL,
	[CreatedBy] [varchar](max) NULL,
	[CreatedOn] [datetime] NULL,
	[PublishDate] [datetime] NULL,
	[DraftDate] [datetime] NULL,
	[ProjectInfo] [varchar](max) NULL,
	[LocNumber] [varchar](max) NULL,
	[PrimaryJvPercentage] [decimal](16, 6) NULL,
	[SecondaryJvPercentage] [decimal](16, 6) NULL,
	[LocAmount] [decimal](16, 6) NULL,
	[TypeOfPackage] [varchar](max) NULL,
	[SerialNo] [int] NULL,
	[NoOfPackage] [int] NULL,
	[PackageDuration] [decimal](16, 6) NULL,
	[LeadMinimumShare] [int] NULL,
	[JvMinShare] [int] NULL,
	[PackageId] [varchar](max) NULL,
	[PackageName] [varchar](max) NULL,
	[PackageValue] [decimal](16, 6) NULL,
	[Index] [int] NULL,
	[PackageDisplayId] [varchar](max) NULL,
	[EstimateValue] [decimal](16, 6) NULL,
	[AverageRequirement] [decimal](16, 6) NULL,
	[CashFlowRequirement] [decimal](16, 6) NULL,
	[PQNo] [varchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BobEProcure_Status]    Script Date: 4/15/2020 4:17:49 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BobEProcure_Status](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PQSupplierId] [varchar](max) NULL,
	[PQId] [varchar](max) NULL,
	[SupplierId] [varchar](max) NULL,
	[ParticipationStatus] [int] NULL,
	[ParticipationDate] [datetime] NULL,
	[ENCStatus] [int] NULL,
	[SuppPQSubmId] [varchar](max) NULL,
	[Status] [int] NULL,
	[DraftStatus] [int] NULL,
	[SubmisionDate] [datetime] NULL,
	[ApplicantType] [varchar](max) NULL,
	[SupplierApprDtlId] [varchar](max) NULL,
	[PreQualificationStatus] [int] NULL,
	[Comments] [varchar](max) NULL,
	[PQNo] [varchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BobEProcure_Vendors]    Script Date: 4/15/2020 4:17:49 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BobEProcure_Vendors](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [varchar](max) NULL,
	[UserName] [varchar](max) NULL,
	[FirstName] [varchar](max) NULL,
	[VendorId] [varchar](max) NULL,
	[Mobile] [varchar](max) NULL,
	[LoggedStatus] [int] NULL,
	[UserStatus] [int] NULL,
	[CompanyId] [varchar](max) NULL,
	[CompanyName] [varchar](max) NULL,
	[CompanyRegistrationNo] [varchar](max) NULL,
	[DirectorName] [varchar](max) NULL,
	[PQCmpAddress] [varchar](max) NULL,
	[PQCmpEmail] [varchar](max) NULL,
	[PQCmpMobile] [varchar](max) NULL,
	[PQCmpWebsite] [varchar](max) NULL,
	[PQCmpPan] [varchar](max) NULL,
	[CompanyShortName] [varchar](max) NULL,
	[cin_no] [varchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Finacle_AMDemands]    Script Date: 4/15/2020 4:17:49 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Finacle_AMDemands](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FORACID] [varchar](max) NULL,
	[AccountName] [varchar](max) NULL,
	[DemandFlowId] [varchar](max) NULL,
	[DemandAmount] [decimal](16, 6) NULL,
	[LastAdjustmentDate] [datetime] NULL,
	[TotalAdjustmentAmount] [decimal](16, 6) NULL,
	[DemandEffectiveDate] [datetime] NULL,
	[CreatedOn] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Finacle_CGS]    Script Date: 4/15/2020 4:17:49 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Finacle_CGS](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FORACID] [varchar](max) NULL,
	[AccountName] [varchar](max) NULL,
	[AccountOpenDate] [datetime] NULL,
	[ClearBalanceAmount] [decimal](16, 6) NULL,
	[AccountMgr] [varchar](max) NULL,
	[CIF] [varchar](200) NULL,
	[SchemeCode] [varchar](200) NULL,
	[Currency] [varchar](200) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Finacle_Contracts]    Script Date: 4/15/2020 4:17:49 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Finacle_Contracts](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CIF] [varchar](max) NULL,
	[ContractId] [varchar](max) NULL,
	[ContractorName] [varchar](max) NULL,
	[ContractValue] [decimal](16, 6) NULL,
	[ContractDate] [datetime] NULL,
	[Prefix] [varchar](max) NULL,
	[Suffix] [varchar](max) NULL,
	[CGS_ID] [varchar](200) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Finacle_LocDetails]    Script Date: 4/15/2020 4:17:49 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Finacle_LocDetails](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[LimitB2KID] [varchar](max) NULL,
	[AccountName] [varchar](max) NULL,
	[FreeText] [varchar](max) NULL,
	[LimitPrefix] [varchar](max) NULL,
	[LimitSuffix] [varchar](max) NULL,
	[SanctionAmount] [decimal](16, 6) NULL,
	[CIF] [varchar](200) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Finacle_LocTransactions]    Script Date: 4/15/2020 4:17:49 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Finacle_LocTransactions](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FORACID] [varchar](max) NULL,
	[CIFID] [varchar](max) NULL,
	[DemandFlow] [varchar](max) NULL,
	[DeleteFlag] [varchar](max) NULL,
	[DemandAmount] [decimal](16, 6) NULL,
	[LastAdjustmentDate] [datetime] NULL,
	[TotalAdjustedAmt] [decimal](16, 6) NULL,
	[CreatedOn] [datetime] NULL,
	[CurrencyCode] [varchar](max) NULL,
	[Prefix] [varchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MonthlyFinacleData]    Script Date: 4/15/2020 4:17:49 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MonthlyFinacleData](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[DataMonth] [datetime] NULL,
	[LOC_Id] [int] NULL,
	[LOC_LocNumber] [varchar](100) NULL,
	[LOC_SanctionedAmount] [decimal](16, 6) NULL,
	[LOC_DisbursedAmount] [decimal](16, 6) NULL,
	[LOC_FirstDisbursement] [datetime] NULL,
	[LOC_ProjectCount] [int] NULL,
	[LOC_AmountRepaid] [decimal](24, 6) NULL,
	[LOC_AmountOutstanding] [decimal](24, 6) NULL,
	[LOC_Principaloverdue] [decimal](24, 6) NULL,
	[LOC_Interestoverdue] [decimal](24, 6) NULL,
	[LOC_PrincipalRepaymentStart] [datetime] NULL,
	[LOC_PrincipalRepaymentEnd] [datetime] NULL,
	[LOC_InterestDueDates] [varchar](50) NULL,
	[LOC_PrincipalDueDates] [varchar](50) NULL,
	[Project_Id] [int] NULL,
	[Project_Code] [varchar](100) NULL,
	[Contract_Id] [int] NULL,
	[Contract_SanctionedAmount] [decimal](16, 6) NULL,
	[Contract_DisbursedAmount] [decimal](16, 6) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TBL_Activity]    Script Date: 4/15/2020 4:17:49 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TBL_Activity](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[EntityId] [int] NOT NULL,
	[EntityName] [varchar](max) NULL,
	[Message] [varchar](max) NULL,
	[CreatedBy] [int] NOT NULL,
	[CreatedOn] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TBL_AuditLogs]    Script Date: 4/15/2020 4:17:49 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TBL_AuditLogs](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TableName] [varchar](50) NULL,
	[ColumnName] [varchar](50) NULL,
	[RecordId] [int] NULL,
	[ActionTaken] [varchar](1) NULL,
	[UserId] [varchar](10) NULL,
	[LoggedOn] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TBL_Classifications]    Script Date: 4/15/2020 4:17:49 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TBL_Classifications](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TBL_Contacts]    Script Date: 4/15/2020 4:17:49 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TBL_Contacts](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NULL,
	[Landline] [nvarchar](15) NULL,
	[Faxno] [nvarchar](15) NULL,
	[MobileNumber] [nvarchar](15) NULL,
	[Email] [nvarchar](100) NULL,
	[AddressLine1] [nvarchar](100) NULL,
	[AddressLine2] [nvarchar](100) NULL,
	[Organization] [nvarchar](100) NULL,
	[Designation] [nvarchar](50) NULL,
	[ContactTypeId] [int] NULL,
	[ContactImg] [varbinary](max) NULL,
	[IsActive] [bit] NULL,
	[City] [nvarchar](50) NULL,
	[PinCode] [nvarchar](10) NULL,
	[CountryId] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TBL_ContactTypes]    Script Date: 4/15/2020 4:17:49 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TBL_ContactTypes](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NULL,
	[IsActive] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TBL_ContractContent]    Script Date: 4/15/2020 4:17:49 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TBL_ContractContent](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ContractId] [int] NOT NULL,
	[Value] [decimal](16, 6) NOT NULL,
	[Percentage] [decimal](5, 2) NULL,
	[isExempt] [bit] NOT NULL,
	[RevisedRequirement] [varchar](max) NULL,
	[MEAApprovalRefNo] [varchar](max) NULL,
	[MEAApprovalDate] [datetime] NULL,
	[Remarks] [varchar](max) NULL,
	[Type] [varchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TBL_ContractLocMap]    Script Date: 4/15/2020 4:17:49 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TBL_ContractLocMap](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ContractId] [int] NOT NULL,
	[LocId] [int] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[Value] [decimal](16, 6) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TBL_ContractResponsibility]    Script Date: 4/15/2020 4:17:49 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TBL_ContractResponsibility](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Responsiblity] [varchar](max) NULL,
	[Authority] [varchar](max) NULL,
	[ContractId] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TBL_ContractTerms]    Script Date: 4/15/2020 4:17:49 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TBL_ContractTerms](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ContractId] [int] NOT NULL,
	[Milestone] [varchar](max) NULL,
	[Percentage] [decimal](5, 2) NULL,
	[Note] [varchar](max) NULL,
	[Sequence] [int] NULL,
	[Amount] [decimal](18, 2) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TBL_EmailRules]    Script Date: 4/15/2020 4:17:49 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TBL_EmailRules](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[OffMonth] [int] NULL,
	[OffWeek] [int] NULL,
	[OffDay] [int] NULL,
	[FreqMonth] [int] NULL,
	[FreqWeek] [int] NULL,
	[FreqDay] [int] NULL,
	[RuleFor] [varchar](50) NULL,
	[RuleName] [varchar](200) NULL,
	[IsActive] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TBL_EmailSchedule]    Script Date: 4/15/2020 4:17:49 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TBL_EmailSchedule](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[SendDate] [datetime] NOT NULL,
	[RecordType] [varchar](20) NULL,
	[RecordId] [int] NULL,
	[TemplateName] [varchar](300) NULL,
	[IsActive] [bit] NOT NULL,
	[Notes] [varchar](500) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TBL_Files]    Script Date: 4/15/2020 4:17:49 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TBL_Files](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Src] [nvarchar](max) NULL,
	[FileFor] [nvarchar](50) NOT NULL,
	[RecordId] [int] NOT NULL,
	[DisplayName] [nvarchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TBL_LOC_Amendments]    Script Date: 4/15/2020 4:17:49 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TBL_LOC_Amendments](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[LOC_Id] [int] NULL,
	[AuditDate] [datetime] NOT NULL,
	[AmendedBy] [varchar](200) NULL,
	[AgreementAmendmentDate] [datetime] NULL,
	[TerminalDate] [date] NULL,
	[IndianContribution] [float] NULL,
	[MdAppDate] [datetime] NULL,
	[DEAAppDate] [datetime] NULL,
	[MEAAppDate] [datetime] NULL,
	[OfferLetterDate] [datetime] NULL,
	[InterestRate] [float] NULL,
	[CommitmentFee] [float] NULL,
	[ManagementFee] [float] NULL,
	[SigningDate] [datetime] NULL,
	[InterestEqualization] [varchar](200) NULL,
	[Type] [varchar](200) NULL,
	[Percentage] [float] NULL,
	[Tenure] [int] NULL,
	[Moratorium] [int] NULL,
	[SpecialCondition] [varchar](max) NULL,
	[OmNumber] [varchar](100) NULL,
	[LOCPurpose] [varchar](max) NULL,
	[InterestType] [varchar](max) NULL,
	[AmendmentNote] [varchar](max) NULL,
	[AmountAllocated] [numeric](38, 2) NULL,
	[MEA_Type] [varchar](max) NULL,
	[MEA_Percentage] [float] NULL,
	[DEA_Type] [varchar](max) NULL,
	[DEA_Percentage] [float] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TBL_LOC_Contract]    Script Date: 4/15/2020 4:17:49 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TBL_LOC_Contract](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[LocId] [int] NOT NULL,
	[ContractId] [int] NOT NULL,
	[Value] [money] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TBL_Logs]    Script Date: 4/15/2020 4:17:49 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TBL_Logs](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Message] [varchar](max) NULL,
	[FullMessage] [varchar](max) NULL,
	[ServiceName] [varchar](max) NULL,
	[CreatedOn] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TBL_MailBody]    Script Date: 4/15/2020 4:17:49 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TBL_MailBody](
	[id] [int] IDENTITY(100,1) NOT NULL,
	[mail_body] [varchar](max) NULL,
	[mail_sent_date] [datetime] NULL,
	[to_address] [varchar](max) NOT NULL,
	[cc] [varchar](max) NULL,
	[bcc] [varchar](max) NULL,
	[status] [varchar](10) NULL,
	[RuleTransactionId] [int] NULL,
	[CreatedOn] [datetime] NOT NULL,
	[MailSubject] [varchar](300) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TBL_Options]    Script Date: 4/15/2020 4:17:49 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TBL_Options](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Type] [int] NOT NULL,
	[Value] [varchar](max) NULL,
	[Parent] [int] NULL,
	[AddedBy] [int] NULL,
	[AddedOn] [datetime] NULL,
	[UpdatedOn] [datetime] NULL,
	[UpdatedBy] [int] NULL,
	[FaIcon] [varchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TBL_Project_Contacts]    Script Date: 4/15/2020 4:17:49 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TBL_Project_Contacts](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ProjectId] [int] NOT NULL,
	[ContactId] [int] NOT NULL,
	[IsActive] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TBL_ProjectLocations]    Script Date: 4/15/2020 4:17:49 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TBL_ProjectLocations](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[SequenceNumber] [int] NOT NULL,
	[Deleted] [int] NULL,
	[Latitude] [varchar](200) NULL,
	[Longitude] [varchar](200) NULL,
	[Color] [varchar](100) NULL,
	[MapType] [varchar](100) NULL,
	[ProjectId] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TBL_ProjectLogs]    Script Date: 4/15/2020 4:17:49 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TBL_ProjectLogs](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ProjectId] [int] NULL,
	[Description] [varchar](max) NULL,
	[UserId] [varchar](10) NULL,
	[LoggedOn] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TBL_ProjectTimeLines]    Script Date: 4/15/2020 4:17:49 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TBL_ProjectTimeLines](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ProjectId] [int] NULL,
	[TimelineTitle] [varchar](50) NULL,
	[TimelineDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TBL_Roles]    Script Date: 4/15/2020 4:17:49 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TBL_Roles](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RoleName] [varchar](max) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TBL_RuleTransactions]    Script Date: 4/15/2020 4:17:49 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TBL_RuleTransactions](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RecordId] [int] NULL,
	[RuleId] [int] NULL,
	[NextRunDate] [datetime] NOT NULL,
	[TriggerDate] [datetime] NOT NULL,
	[LastRunDate] [datetime] NULL,
	[IsActive] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TBL_StakeHolders]    Script Date: 4/15/2020 4:17:49 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TBL_StakeHolders](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Type] [int] NOT NULL,
	[UserId] [varchar](7) NOT NULL,
	[ProjectId] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TBL_String_Mapper]    Script Date: 4/15/2020 4:17:49 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TBL_String_Mapper](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Key] [nvarchar](50) NULL,
	[Value] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TBL_SyncLog]    Script Date: 4/15/2020 4:17:49 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TBL_SyncLog](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[System] [varchar](100) NULL,
	[Status] [varchar](100) NULL,
	[ServiceName] [varchar](200) NULL,
	[FullMessage] [varchar](max) NULL,
	[CreatedOn] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TBl_TeamMapping]    Script Date: 4/15/2020 4:17:49 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TBl_TeamMapping](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[EntityId] [int] NOT NULL,
	[EntityName] [varchar](max) NOT NULL,
	[UserId] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TBL_Terms]    Script Date: 4/15/2020 4:17:49 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TBL_Terms](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[InterestRate] [float] NOT NULL,
	[CommitmentFee] [float] NOT NULL,
	[ManagementFee] [float] NOT NULL,
	[Tenure] [int] NOT NULL,
	[Moratorium] [int] NOT NULL,
	[IndianContribution] [float] NOT NULL,
	[SpecialConditions] [varchar](max) NULL,
	[CurrencyCode] [varchar](3) NULL,
	[Type] [varchar](max) NULL,
	[ApprovalType] [varchar](max) NULL,
	[Percentage] [float] NOT NULL,
	[LOCClassification] [varchar](200) NULL,
	[RiskClassification] [varchar](200) NULL,
	[AddedBy] [int] NULL,
	[AddedOn] [datetime] NULL,
	[UpdatedOn] [datetime] NULL,
	[UpdatedBy] [int] NULL,
	[InterestType] [varchar](max) NULL,
 CONSTRAINT [PK__TBL_Term__3214EC077D4546CD] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TBL_Terms_Country]    Script Date: 4/15/2020 4:17:49 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TBL_Terms_Country](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RiskCategoryClass] [varchar](200) NULL,
	[LocGuideLinesClass] [varchar](200) NULL,
	[InterestRate] [float] NOT NULL,
	[CommitmentFee] [float] NOT NULL,
	[ManagementFee] [float] NOT NULL,
	[Tenure] [int] NOT NULL,
	[Moratorium] [int] NOT NULL,
	[IndianContribution] [float] NOT NULL,
	[SpecialConditions] [varchar](max) NULL,
	[CurrecyCode] [varchar](3) NULL,
	[CountryId] [int] NOT NULL,
	[ApprovalType] [varchar](max) NULL,
	[Type] [varchar](max) NULL,
	[Percentage] [float] NOT NULL,
	[InterestType] [varchar](max) NULL,
 CONSTRAINT [PK__TBL_Term__3214EC0737D57D0A] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TBL_UserRoleMap]    Script Date: 4/15/2020 4:17:49 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TBL_UserRoleMap](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[RoleId] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TBL_Users]    Script Date: 4/15/2020 4:17:49 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TBL_Users](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Username] [varchar](max) NOT NULL,
	[EmployeeNo] [varchar](max) NULL,
	[Department] [varchar](max) NULL,
	[DisplayName] [varchar](max) NULL,
	[EmailAddress] [varchar](max) NULL,
	[ContactId] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [Location].[CountryCodes]    Script Date: 4/15/2020 4:17:49 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Location].[CountryCodes](
	[CountryCodeID] [nvarchar](2) NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[CountryCodeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [Location].[GeoCodes]    Script Date: 4/15/2020 4:17:49 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Location].[GeoCodes](
	[GeoID] [bigint] IDENTITY(1,1) NOT NULL,
	[ProjectID] [varchar](6) NOT NULL,
	[LocationPoint] [varchar](max) NOT NULL,
	[CountryCodeID] [nvarchar](2) NULL,
	[Administrative] [nvarchar](255) NOT NULL,
	[Longitude] [float] NOT NULL,
	[Latitude] [float] NOT NULL,
	[PrecisionID] [int] NOT NULL,
	[LocationTypeID] [nvarchar](255) NOT NULL,
	[Confirmed] [bit] NOT NULL,
	[Deleted] [int] NOT NULL,
	[Percentage] [float] NULL,
	[CMP] [bit] NULL,
	[StartDate] [datetime] NOT NULL,
	[EndDate] [datetime] NULL,
	[UserID] [varchar](255) NOT NULL,
	[LastUpdated] [datetime] NOT NULL,
 CONSTRAINT [PK_ProjectLocations] PRIMARY KEY CLUSTERED 
(
	[GeoID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [Location].[LocationType]    Script Date: 4/15/2020 4:17:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Location].[LocationType](
	[LocationTypeID] [nvarchar](255) NOT NULL,
	[Name] [varchar](255) NOT NULL,
	[Description] [varchar](1000) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[LocationTypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [Location].[Precision]    Script Date: 4/15/2020 4:17:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Location].[Precision](
	[PrecisionID] [int] NOT NULL,
	[Name] [varchar](255) NOT NULL,
	[Description] [varchar](1000) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[PrecisionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [Project].[AuditedFinancialStatements]    Script Date: 4/15/2020 4:17:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Project].[AuditedFinancialStatements](
	[ProjectID] [varchar](6) NOT NULL,
	[StatementID] [int] NOT NULL,
	[DueDate] [datetime] NULL,
	[PromptDate] [datetime] NULL,
	[ReceivedDate] [datetime] NULL,
	[PeriodFrom] [datetime] NULL,
	[PeriodTo] [datetime] NULL,
	[Value] [decimal](18, 0) NULL,
	[Currency] [varchar](3) NULL,
	[StatementType] [varchar](255) NULL,
	[reason_action] [varchar](1000) NULL,
	[Status] [varchar](6) NULL,
	[LastUpdated] [datetime] NULL,
	[UserID] [varchar](6) NULL,
	[DocumentID] [varchar](12) NULL,
	[DocSource] [varchar](1) NULL,
PRIMARY KEY CLUSTERED 
(
	[ProjectID] ASC,
	[StatementID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [Project].[AuditedFinancialStatementsTemp]    Script Date: 4/15/2020 4:17:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Project].[AuditedFinancialStatementsTemp](
	[ProjectID] [varchar](6) NOT NULL,
	[StatementID] [int] NOT NULL,
	[DueDate] [datetime] NULL,
	[PromptDate] [datetime] NULL,
	[ReceivedDate] [datetime] NULL,
	[PeriodFrom] [datetime] NULL,
	[PeriodTo] [datetime] NULL,
	[Value] [decimal](18, 0) NULL,
	[Currency] [varchar](3) NULL,
	[StatementType] [varchar](255) NULL,
	[reason_action] [varchar](1000) NULL,
	[Status] [varchar](6) NULL,
	[LastUpdated] [datetime] NULL,
	[UserID] [varchar](6) NULL,
	[DocumentID] [int] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [Project].[ConditionalityReview]    Script Date: 4/15/2020 4:17:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Project].[ConditionalityReview](
	[ProjectID] [varchar](6) NOT NULL,
	[DisbursementSuspended] [varchar](255) NULL,
	[cause] [varchar](1000) NULL,
	[date_suspended] [datetime] NULL,
	[consequences] [varchar](1000) NULL,
	[Status] [varchar](1) NULL,
	[LastUpdated] [datetime] NULL,
	[UserID] [varchar](6) NULL,
PRIMARY KEY CLUSTERED 
(
	[ProjectID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [Project].[Deferral]    Script Date: 4/15/2020 4:17:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Project].[Deferral](
	[ProjectID] [varchar](6) NOT NULL,
	[DeferralTimescale] [varchar](3) NULL,
	[DeferralReason] [varchar](1000) NULL,
	[DeferralJustification] [varchar](1000) NULL,
	[AnnualReviewDate] [datetime] NULL,
	[PCRDueDate] [datetime] NULL,
	[LastUpdated] [datetime] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [Project].[DeferralReason]    Script Date: 4/15/2020 4:17:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Project].[DeferralReason](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[DeferralReasons] [varchar](150) NULL,
	[IsActive] [bit] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [Project].[DSOMarkers]    Script Date: 4/15/2020 4:17:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Project].[DSOMarkers](
	[ProjectID] [varchar](6) NOT NULL,
	[GrowGovTradeBaseServ] [varchar](255) NULL,
	[ClimateChange] [varchar](255) NULL,
	[ConflictHumaniterian] [varchar](255) NULL,
	[GlobalPartnerships] [varchar](255) NULL,
	[MoreEffectiveDoners] [varchar](255) NULL,
	[HighQualityAid] [varchar](255) NULL,
	[InternalEfficency] [varchar](255) NULL,
	[Status] [varchar](1) NULL,
	[LastUpdated] [datetime] NULL,
	[UserID] [varchar](6) NULL,
PRIMARY KEY CLUSTERED 
(
	[ProjectID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [Project].[Evaluation]    Script Date: 4/15/2020 4:17:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Project].[Evaluation](
	[EvaluationID] [int] IDENTITY(1,1) NOT NULL,
	[ProjectID] [varchar](6) NOT NULL,
	[EvaluationTypeID] [varchar](6) NOT NULL,
	[IfOther] [varchar](255) NULL,
	[ManagementOfEvaluation] [varchar](6) NULL,
	[EstimatedBudget] [decimal](13, 0) NULL,
	[StartDate] [datetime] NULL,
	[EndDate] [datetime] NULL,
	[AdditionalInfo] [varchar](1000) NULL,
	[LastUpdated] [datetime] NULL,
	[UserID] [varchar](6) NULL,
PRIMARY KEY CLUSTERED 
(
	[EvaluationID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [Project].[EvaluationDocuments]    Script Date: 4/15/2020 4:17:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Project].[EvaluationDocuments](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[EvaluationID] [int] NOT NULL,
	[DocumentID] [varchar](12) NOT NULL,
	[Description] [varchar](max) NOT NULL,
	[Status] [varchar](255) NULL,
	[LastUpdate] [datetime] NULL,
	[UserID] [varchar](6) NULL,
	[DocSource] [varchar](1) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [Project].[EvaluationDocumentsTemp]    Script Date: 4/15/2020 4:17:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Project].[EvaluationDocumentsTemp](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[EvaluationID] [int] NOT NULL,
	[DocumentID] [varchar](7) NOT NULL,
	[Description] [varchar](max) NOT NULL,
	[Status] [varchar](255) NULL,
	[LastUpdate] [datetime] NULL,
	[UserID] [varchar](6) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [Project].[Markers]    Script Date: 4/15/2020 4:17:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Project].[Markers](
	[ProjectID] [varchar](6) NOT NULL,
	[GenderEquality] [varchar](255) NULL,
	[HIVAIDS] [varchar](255) NULL,
	[Biodiversity] [varchar](255) NULL,
	[Mitigation] [varchar](255) NULL,
	[Adaptation] [varchar](255) NULL,
	[Desertification] [varchar](255) NULL,
	[Status] [varchar](255) NULL,
	[LastUpdated] [datetime] NULL,
	[UserID] [varchar](6) NULL,
	[Disability] [varchar](255) NULL,
	[DisabilityPercentage] [int] NULL,
 CONSTRAINT [Project.Markers_pk] PRIMARY KEY CLUSTERED 
(
	[ProjectID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [Project].[OverallRiskRating]    Script Date: 4/15/2020 4:17:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Project].[OverallRiskRating](
	[OverallRiskRatingId] [int] IDENTITY(1,1) NOT NULL,
	[ProjectID] [varchar](6) NOT NULL,
	[Comments] [varchar](1000) NULL,
	[RiskScore] [varchar](10) NULL,
	[UserID] [varchar](6) NULL,
	[LastUpdated] [datetime] NULL,
 CONSTRAINT [PK_OverallRiskRating] PRIMARY KEY CLUSTERED 
(
	[OverallRiskRatingId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [Project].[ProjectBudgetCentreOrgUnit]    Script Date: 4/15/2020 4:17:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Project].[ProjectBudgetCentreOrgUnit](
	[Identity] [int] IDENTITY(1,1) NOT NULL,
	[ProjectID] [varchar](6) NOT NULL,
	[BudgetCentre] [varchar](5) NULL,
	[OrgUnit] [varchar](5) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [Project].[ProjectDates]    Script Date: 4/15/2020 4:17:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Project].[ProjectDates](
	[ProjectID] [varchar](6) NOT NULL,
	[Created_date] [datetime] NULL,
	[FinancialStartDate] [datetime] NULL,
	[FinancialEndDate] [datetime] NULL,
	[OperationalStartDate] [datetime] NULL,
	[OperationalEndDate] [datetime] NULL,
	[ActualStartDate] [datetime] NULL,
	[PromptCompletionDate] [datetime] NULL,
	[ESNApprovedDate] [datetime] NULL,
	[LastUpdate] [datetime] NULL,
	[UserID] [varchar](6) NULL,
	[ActualEndDate] [datetime] NULL,
	[ArchiveDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[ProjectID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [Project].[ProjectInfo]    Script Date: 4/15/2020 4:17:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Project].[ProjectInfo](
	[ProjectID] [varchar](6) NOT NULL,
	[OVIS] [varchar](max) NULL,
	[TeamMarker] [varchar](255) NULL,
	[RiskAtApproval] [varchar](255) NULL,
	[SpecificConditions] [varchar](255) NULL,
	[LastUpdate] [datetime] NULL,
	[UserID] [varchar](6) NULL,
PRIMARY KEY CLUSTERED 
(
	[ProjectID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [Project].[Reports]    Script Date: 4/15/2020 4:17:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Project].[Reports](
	[ProjectID] [varchar](6) NOT NULL,
	[ReportID] [int] NOT NULL,
	[ReportType] [varchar](1000) NULL,
	[DueDate] [datetime] NULL,
	[PromptDate] [datetime] NULL,
	[ReceivedDate] [datetime] NULL,
	[QuestNo] [int] NULL,
	[Status] [varchar](6) NULL,
	[LastUpdated] [datetime] NULL,
	[UserID] [varchar](6) NULL,
PRIMARY KEY CLUSTERED 
(
	[ProjectID] ASC,
	[ReportID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [Project].[ReviewARScore]    Script Date: 4/15/2020 4:17:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Project].[ReviewARScore](
	[ProjectID] [varchar](6) NOT NULL,
	[ReviewID] [int] NOT NULL,
	[OverallScore] [varchar](3) NULL,
	[Justification] [varchar](1000) NULL,
	[Progress] [varchar](1000) NULL,
	[OnTrackTime] [varchar](1) NULL,
	[OnTrackCost] [varchar](1) NULL,
	[OffTrackJustification] [varchar](1000) NULL,
	[Status] [varchar](6) NULL,
	[LastUpdated] [datetime] NULL,
	[UserID] [varchar](20) NULL,
PRIMARY KEY CLUSTERED 
(
	[ProjectID] ASC,
	[ReviewID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [Project].[ReviewDocuments]    Script Date: 4/15/2020 4:17:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Project].[ReviewDocuments](
	[ReviewDocumentsID] [int] IDENTITY(1,1) NOT NULL,
	[ProjectID] [varchar](6) NOT NULL,
	[ReviewID] [int] NOT NULL,
	[DocumentID] [varchar](12) NOT NULL,
	[Description] [varchar](max) NOT NULL,
	[LastUpdate] [datetime] NULL,
	[UserID] [varchar](6) NULL,
	[DocSource] [varchar](1) NULL,
 CONSTRAINT [PK_ReviewDocuments] PRIMARY KEY CLUSTERED 
(
	[ReviewDocumentsID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [Project].[ReviewDocumentsTemp]    Script Date: 4/15/2020 4:17:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Project].[ReviewDocumentsTemp](
	[ReviewDocumentsID] [int] IDENTITY(1,1) NOT NULL,
	[ProjectID] [varchar](6) NOT NULL,
	[ReviewID] [int] NOT NULL,
	[DocumentID] [varchar](7) NOT NULL,
	[Description] [varchar](max) NOT NULL,
	[LastUpdate] [datetime] NULL,
	[UserID] [varchar](6) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [Project].[ReviewOutputs]    Script Date: 4/15/2020 4:17:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Project].[ReviewOutputs](
	[ProjectID] [varchar](6) NOT NULL,
	[ReviewID] [int] NOT NULL,
	[OutputID] [int] NOT NULL,
	[OutputDescription] [varchar](500) NULL,
	[Weight] [int] NULL,
	[OutputScore] [varchar](3) NULL,
	[ImpactScore] [float] NULL,
	[Risk] [varchar](10) NULL,
	[Status] [varchar](6) NULL,
	[LastUpdated] [datetime] NULL,
	[UserID] [varchar](6) NULL,
PRIMARY KEY CLUSTERED 
(
	[ProjectID] ASC,
	[ReviewID] ASC,
	[OutputID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [Project].[ReviewPCRScore]    Script Date: 4/15/2020 4:17:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Project].[ReviewPCRScore](
	[ProjectID] [varchar](6) NOT NULL,
	[ReviewID] [int] NOT NULL,
	[FinalOutputScore] [varchar](3) NULL,
	[OutcomeScore] [varchar](3) NULL,
	[ProgressToImpact] [varchar](1000) NULL,
	[CompletedToTimescales] [varchar](1) NULL,
	[CompletedToCost] [varchar](1) NULL,
	[FailedJustification] [varchar](1000) NULL,
	[Status] [varchar](6) NULL,
	[LastUpdated] [datetime] NULL,
	[UserID] [varchar](6) NULL,
PRIMARY KEY CLUSTERED 
(
	[ProjectID] ASC,
	[ReviewID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [Project].[ReviewScore]    Script Date: 4/15/2020 4:17:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Project].[ReviewScore](
	[OutputScore] [varchar](3) NOT NULL,
	[Definition] [nvarchar](50) NOT NULL,
	[Weight] [int] NOT NULL,
 CONSTRAINT [PK_ReviewScore] PRIMARY KEY CLUSTERED 
(
	[OutputScore] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [Project].[ReviewStage]    Script Date: 4/15/2020 4:17:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Project].[ReviewStage](
	[StageID] [varchar](5) NOT NULL,
	[StageDescription] [varchar](255) NULL,
	[Status] [varchar](1) NULL,
	[LastUpdated] [datetime] NULL,
	[UserID] [varchar](6) NULL,
PRIMARY KEY CLUSTERED 
(
	[StageID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [Project].[RiskDocument]    Script Date: 4/15/2020 4:17:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Project].[RiskDocument](
	[RiskRegisterID] [int] IDENTITY(1,1) NOT NULL,
	[ProjectID] [varchar](6) NOT NULL,
	[DocumentID] [varchar](12) NOT NULL,
	[Description] [varchar](max) NOT NULL,
	[LastUpdate] [datetime] NULL,
	[UserID] [varchar](6) NULL,
	[DocSource] [varchar](1) NULL,
 CONSTRAINT [PK_RiskRegisterDocuments] PRIMARY KEY CLUSTERED 
(
	[RiskRegisterID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [Project].[RiskDocumentTemp]    Script Date: 4/15/2020 4:17:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Project].[RiskDocumentTemp](
	[RiskRegisterID] [int] IDENTITY(1,1) NOT NULL,
	[ProjectID] [varchar](6) NOT NULL,
	[DocumentID] [varchar](7) NOT NULL,
	[Description] [varchar](max) NOT NULL,
	[LastUpdate] [datetime] NULL,
	[UserID] [varchar](6) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [Project].[RiskRegister]    Script Date: 4/15/2020 4:17:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Project].[RiskRegister](
	[RiskID] [int] IDENTITY(1,1) NOT NULL,
	[ProjectID] [varchar](6) NOT NULL,
	[RiskDescription] [varchar](max) NULL,
	[Owner] [varchar](6) NOT NULL,
	[RiskCategory] [int] NOT NULL,
	[RiskLikelihood] [int] NULL,
	[RiskImpact] [int] NULL,
	[GrossRisk] [varchar](10) NOT NULL,
	[MitigationStrategy] [varchar](max) NULL,
	[ResidualLikelihood] [int] NULL,
	[ResidualImpact] [int] NULL,
	[ResidualRisk] [varchar](10) NOT NULL,
	[Comments] [varchar](max) NULL,
	[ExternalOwner] [varchar](max) NULL,
	[Status] [varchar](1) NOT NULL,
	[LastUpdated] [datetime] NOT NULL,
	[UserID] [varchar](6) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[RiskID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [Project].[TeamExternal]    Script Date: 4/15/2020 4:17:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Project].[TeamExternal](
	[ProjectID] [varchar](6) NOT NULL,
	[MemberID] [varchar](7) NOT NULL,
	[MemberDescription] [varchar](255) NULL,
	[Status] [varchar](1) NULL,
	[LastUpdated] [datetime] NULL,
	[UserID] [varchar](6) NULL,
PRIMARY KEY CLUSTERED 
(
	[ProjectID] ASC,
	[MemberID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [Results].[OutputIndicator]    Script Date: 4/15/2020 4:17:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Results].[OutputIndicator](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ProjectOutputID] [int] NULL,
	[OutputIndicatorID] [int] NULL,
	[IndicatorDescription] [varchar](1000) NULL,
	[Source] [varchar](1000) NULL,
	[Units] [varchar](255) NULL,
	[Baseline] [varchar](255) NULL,
	[BaselineDate] [datetime] NULL,
	[Target] [varchar](255) NULL,
	[TargetDate] [datetime] NULL,
	[TargetAchieved] [varchar](255) NULL,
	[IsDRF] [varchar](1) NULL,
	[IsCHR] [varchar](1) NULL,
	[Status] [varchar](1) NULL,
	[LastUpdated] [datetime] NULL,
	[UserID] [varchar](6) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [Results].[OutputIndicatorMilestones]    Script Date: 4/15/2020 4:17:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Results].[OutputIndicatorMilestones](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[OutputIndicatorID] [int] NULL,
	[MilestoneID] [int] NULL,
	[From] [datetime] NULL,
	[To] [datetime] NULL,
	[Planned] [varchar](255) NULL,
	[Change] [varchar](255) NULL,
	[Achieved] [varchar](255) NULL,
	[Status] [varchar](1) NULL,
	[LastUpdated] [datetime] NULL,
	[UserID] [varchar](6) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [Results].[ProjectOutput]    Script Date: 4/15/2020 4:17:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Results].[ProjectOutput](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ProjectID] [varchar](6) NULL,
	[OutputID] [int] NULL,
	[ProjectOutputDescription] [varchar](1000) NULL,
	[Assumption] [varchar](1000) NULL,
	[ImpactWeightingPercentage] [int] NULL,
	[Status] [varchar](1) NULL,
	[LastUpdated] [datetime] NULL,
	[UserID] [varchar](6) NULL,
	[Risk] [varchar](6) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [Shadow].[AuditedFinancialStatements]    Script Date: 4/15/2020 4:17:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Shadow].[AuditedFinancialStatements](
	[Identity] [int] IDENTITY(1,1) NOT NULL,
	[Change_Status] [varchar](255) NOT NULL,
	[ProjectID] [varchar](6) NOT NULL,
	[StatementID] [int] NOT NULL,
	[DueDate] [datetime] NULL,
	[PromptDate] [datetime] NULL,
	[ReceivedDate] [datetime] NULL,
	[PeriodFrom] [datetime] NULL,
	[PeriodTo] [datetime] NULL,
	[Value] [decimal](18, 0) NULL,
	[Currency] [varchar](3) NULL,
	[StatementType] [varchar](255) NULL,
	[reason_action] [varchar](1000) NULL,
	[Status] [varchar](255) NULL,
	[LastUpdated] [datetime] NOT NULL,
	[UserID] [varchar](6) NULL,
	[DocumentID] [varchar](12) NULL,
PRIMARY KEY CLUSTERED 
(
	[Identity] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [Shadow].[AuditedFinancialStatementsTemp]    Script Date: 4/15/2020 4:17:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Shadow].[AuditedFinancialStatementsTemp](
	[Identity] [int] IDENTITY(1,1) NOT NULL,
	[Change_Status] [varchar](255) NOT NULL,
	[ProjectID] [varchar](6) NOT NULL,
	[StatementID] [int] NOT NULL,
	[DueDate] [datetime] NULL,
	[PromptDate] [datetime] NULL,
	[ReceivedDate] [datetime] NULL,
	[PeriodFrom] [datetime] NULL,
	[PeriodTo] [datetime] NULL,
	[Value] [decimal](18, 0) NULL,
	[Currency] [varchar](3) NULL,
	[StatementType] [varchar](255) NULL,
	[reason_action] [varchar](1000) NULL,
	[Status] [varchar](255) NULL,
	[LastUpdated] [datetime] NOT NULL,
	[UserID] [varchar](6) NULL,
	[DocumentID] [int] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [Shadow].[ComponentDates]    Script Date: 4/15/2020 4:17:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Shadow].[ComponentDates](
	[Identity] [int] IDENTITY(1,1) NOT NULL,
	[Change_Status] [varchar](255) NOT NULL,
	[ComponentID] [varchar](10) NOT NULL,
	[StartDate] [datetime] NULL,
	[EndDate] [datetime] NULL,
	[OperationalStartDate] [datetime] NULL,
	[OperationalEndDate] [datetime] NULL,
	[LastUpdate] [datetime] NULL,
	[UserID] [varchar](6) NULL,
PRIMARY KEY CLUSTERED 
(
	[Identity] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [Shadow].[ComponentDeliveryChain]    Script Date: 4/15/2020 4:17:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Shadow].[ComponentDeliveryChain](
	[Identity] [int] IDENTITY(1,1) NOT NULL,
	[ID] [int] NOT NULL,
	[ChangeStatus] [varchar](255) NOT NULL,
	[ComponentID] [varchar](10) NOT NULL,
	[ChainID] [varchar](50) NULL,
	[ParentID] [int] NULL,
	[ParentType] [varchar](1) NULL,
	[ChildID] [int] NULL,
	[ChildType] [varchar](1) NULL,
	[ParentNodeID] [int] NULL,
	[LastUpdate] [datetime] NULL,
	[UserID] [varchar](255) NULL,
	[Status] [varchar](1) NULL,
 CONSTRAINT [PK_DeliveryChain] PRIMARY KEY CLUSTERED 
(
	[Identity] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [Shadow].[ComponentDeliveryChainBackup]    Script Date: 4/15/2020 4:17:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Shadow].[ComponentDeliveryChainBackup](
	[Identity] [int] IDENTITY(1,1) NOT NULL,
	[ID] [int] NOT NULL,
	[ChangeStatus] [varchar](255) NOT NULL,
	[ComponentID] [varchar](10) NULL,
	[ChainID] [varchar](50) NULL,
	[ParentID] [int] NULL,
	[ParentType] [varchar](1) NULL,
	[ChildID] [int] NULL,
	[ChildType] [varchar](1) NULL,
	[ParentNodeID] [int] NULL,
	[LastUpdate] [datetime] NULL,
	[UserID] [varchar](255) NULL,
	[Status] [varchar](1) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [Shadow].[ComponentMaster]    Script Date: 4/15/2020 4:17:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Shadow].[ComponentMaster](
	[Identity] [int] IDENTITY(1,1) NOT NULL,
	[Change_Status] [varchar](255) NOT NULL,
	[ComponentID] [varchar](10) NOT NULL,
	[ComponentDescription] [varchar](max) NULL,
	[BudgetCentreID] [varchar](5) NULL,
	[ProjectID] [varchar](6) NULL,
	[AdminApprover] [varchar](7) NULL,
	[FundingMechanism] [varchar](255) NULL,
	[OperationalStatus] [varchar](255) NULL,
	[BenefittingCountry] [varchar](2) NULL,
	[LastUpdate] [datetime] NULL,
	[UserID] [varchar](6) NULL,
	[Approved] [varchar](5) NULL,
	[FundingArrangementValue] [varchar](20) NULL,
	[PartnerOrganisationValue] [varchar](20) NULL,
PRIMARY KEY CLUSTERED 
(
	[Identity] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [Shadow].[Evaluation]    Script Date: 4/15/2020 4:17:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Shadow].[Evaluation](
	[Identity] [int] IDENTITY(1,1) NOT NULL,
	[Change_Status] [varchar](255) NOT NULL,
	[EvaluationID] [int] NOT NULL,
	[ProjectID] [varchar](6) NOT NULL,
	[EvaluationTypeID] [varchar](6) NOT NULL,
	[IfOther] [varchar](255) NULL,
	[ManagementOfEvaluation] [varchar](6) NULL,
	[EstimatedBudget] [decimal](13, 0) NULL,
	[StartDate] [datetime] NULL,
	[EndDate] [datetime] NULL,
	[AdditionalInfo] [varchar](1000) NULL,
	[LastUpdated] [datetime] NULL,
	[UserID] [varchar](6) NULL,
PRIMARY KEY CLUSTERED 
(
	[Identity] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [Shadow].[ImplementingOrganisation]    Script Date: 4/15/2020 4:17:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Shadow].[ImplementingOrganisation](
	[Identity] [int] IDENTITY(1,1) NOT NULL,
	[Change_Status] [varchar](255) NOT NULL,
	[ComponentID] [varchar](10) NOT NULL,
	[LineNo] [int] NOT NULL,
	[ImplementingOrganisation] [varchar](255) NULL,
	[LastUpdate] [datetime] NULL,
	[UserID] [varchar](6) NULL,
PRIMARY KEY CLUSTERED 
(
	[Identity] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [Shadow].[InputSectorCodes]    Script Date: 4/15/2020 4:17:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Shadow].[InputSectorCodes](
	[Identity] [int] IDENTITY(1,1) NOT NULL,
	[Change_Status] [varchar](255) NOT NULL,
	[ComponentID] [varchar](10) NOT NULL,
	[LineNo] [int] NOT NULL,
	[InputSectorCode] [varchar](7) NULL,
	[Percentage] [int] NULL,
	[LastUpdate] [datetime] NULL,
	[UserID] [varchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[Identity] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [Shadow].[Markers]    Script Date: 4/15/2020 4:17:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Shadow].[Markers](
	[Identity] [int] IDENTITY(1,1) NOT NULL,
	[Change_Status] [varchar](255) NOT NULL,
	[ComponentID] [varchar](10) NOT NULL,
	[PBA] [varchar](3) NOT NULL,
	[SWAP] [varchar](3) NOT NULL,
	[Status] [varchar](1) NULL,
	[LastUpdate] [datetime] NULL,
	[UserID] [varchar](6) NULL,
PRIMARY KEY CLUSTERED 
(
	[Identity] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [Shadow].[Performance]    Script Date: 4/15/2020 4:17:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Shadow].[Performance](
	[Identity] [int] IDENTITY(1,1) NOT NULL,
	[Change_Status] [varchar](255) NOT NULL,
	[ProjectID] [varchar](6) NOT NULL,
	[ARRequired] [varchar](3) NULL,
	[ARExemptJustification] [varchar](1000) NULL,
	[ARDueDate] [datetime] NULL,
	[ARPromptDate] [datetime] NULL,
	[ARDefferal] [varchar](3) NULL,
	[PCRRequired] [varchar](3) NULL,
	[PCRExemptJustification] [varchar](1000) NULL,
	[PCRDueDate] [datetime] NULL,
	[PCRPrompt] [datetime] NULL,
	[PCRDefferal] [varchar](3) NULL,
	[PCRDefferalJustification] [varchar](1000) NULL,
	[PCRAuthorised] [varchar](3) NULL,
	[ARExcemptReason] [varchar](1000) NULL,
	[PCRExcemptReason] [varchar](1000) NULL,
	[DefferalTimeScale] [int] NULL,
	[DeferralReason] [varchar](1000) NULL,
	[Status] [varchar](6) NULL,
	[LastUpdated] [datetime] NULL,
	[UserID] [varchar](6) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [Shadow].[ProjectDates]    Script Date: 4/15/2020 4:17:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Shadow].[ProjectDates](
	[Identity] [int] IDENTITY(1,1) NOT NULL,
	[Change_Status] [varchar](255) NOT NULL,
	[ProjectID] [varchar](6) NOT NULL,
	[Created_date] [datetime] NULL,
	[FinancialStartDate] [datetime] NULL,
	[FinancialEndDate] [datetime] NULL,
	[OperationalStartDate] [datetime] NULL,
	[OperationalEndDate] [datetime] NULL,
	[ActualStartDate] [datetime] NULL,
	[PromptCompletionDate] [datetime] NULL,
	[ESNApprovedDate] [datetime] NULL,
	[LastUpdate] [datetime] NULL,
	[UserID] [varchar](6) NULL,
	[ActualEndDate] [datetime] NULL,
	[ArchiveDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[Identity] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [Shadow].[ProjectInfo]    Script Date: 4/15/2020 4:17:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Shadow].[ProjectInfo](
	[ProjectID] [varchar](6) NOT NULL,
	[Change_Status] [varchar](255) NOT NULL,
	[OVIS] [varchar](max) NULL,
	[TeamMarker] [varchar](255) NULL,
	[RiskAtApproval] [varchar](255) NULL,
	[SpecificConditions] [varchar](255) NULL,
	[LastUpdated] [datetime] NOT NULL,
	[UserID] [varchar](6) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [Shadow].[ProjectMarkers]    Script Date: 4/15/2020 4:17:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Shadow].[ProjectMarkers](
	[Identity] [int] IDENTITY(1,1) NOT NULL,
	[ChangeStatus] [varchar](255) NULL,
	[ProjectID] [varchar](6) NOT NULL,
	[GenderEquality] [varchar](255) NULL,
	[HIVAIDS] [varchar](255) NULL,
	[Biodiversity] [varchar](255) NULL,
	[Mitigation] [varchar](255) NULL,
	[Adaptation] [varchar](255) NULL,
	[Desertification] [varchar](255) NULL,
	[Status] [varchar](255) NULL,
	[LastUpdated] [datetime] NULL,
	[UserID] [varchar](6) NULL,
 CONSTRAINT [Shadow.ProjectMarkers_pk] PRIMARY KEY CLUSTERED 
(
	[Identity] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [Shadow].[ProjectMaster]    Script Date: 4/15/2020 4:17:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Shadow].[ProjectMaster](
	[Identity] [int] IDENTITY(1,1) NOT NULL,
	[Change_Status] [varchar](255) NOT NULL,
	[ProjectID] [varchar](6) NOT NULL,
	[Title] [varchar](max) NULL,
	[Description] [varchar](max) NULL,
	[BudgetCentreID] [varchar](5) NULL,
	[Stage] [varchar](5) NULL,
	[Status] [varchar](255) NULL,
	[LastUpdate] [datetime] NULL,
	[UserID] [varchar](6) NULL,
PRIMARY KEY CLUSTERED 
(
	[Identity] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [Shadow].[ReviewARScore]    Script Date: 4/15/2020 4:17:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Shadow].[ReviewARScore](
	[ProjectID] [varchar](6) NOT NULL,
	[ReviewID] [int] NOT NULL,
	[OverallScore] [varchar](3) NULL,
	[Justification] [varchar](1000) NULL,
	[Progress] [varchar](1000) NULL,
	[OnTrackTime] [varchar](1) NULL,
	[OnTrackCost] [varchar](1) NULL,
	[OffTrackJustification] [varchar](1000) NULL,
	[Status] [varchar](6) NULL,
	[LastUpdated] [datetime] NULL,
	[UserID] [varchar](20) NULL,
	[Change_Status] [varchar](10) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [Shadow].[ReviewDeferral]    Script Date: 4/15/2020 4:17:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Shadow].[ReviewDeferral](
	[DeferralID] [int] NOT NULL,
	[ProjectID] [varchar](6) NOT NULL,
	[ReviewID] [int] NOT NULL,
	[StageID] [varchar](5) NOT NULL,
	[DeferralTimescale] [varchar](3) NULL,
	[DeferralJustification] [varchar](500) NULL,
	[ApproverComment] [varchar](500) NULL,
	[Approver] [varchar](6) NULL,
	[Approved] [varchar](1) NULL,
	[Requester] [varchar](6) NULL,
	[PreviousReviewDate] [datetime] NULL,
	[LastUpdated] [datetime] NULL,
	[UpdatedBy] [varchar](10) NULL,
	[Change_Status] [varchar](10) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [Shadow].[ReviewDocuments]    Script Date: 4/15/2020 4:17:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Shadow].[ReviewDocuments](
	[ReviewDocumentsID] [int] NOT NULL,
	[ProjectID] [varchar](6) NOT NULL,
	[ReviewID] [int] NOT NULL,
	[DocumentID] [varchar](12) NOT NULL,
	[Description] [varchar](max) NOT NULL,
	[LastUpdate] [datetime] NULL,
	[UserID] [varchar](6) NULL,
	[Change_Status] [varchar](10) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [Shadow].[ReviewDocumentsTemp]    Script Date: 4/15/2020 4:17:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Shadow].[ReviewDocumentsTemp](
	[ReviewDocumentsID] [int] NOT NULL,
	[ProjectID] [varchar](6) NOT NULL,
	[ReviewID] [int] NOT NULL,
	[DocumentID] [varchar](7) NOT NULL,
	[Description] [varchar](max) NOT NULL,
	[LastUpdate] [datetime] NULL,
	[UserID] [varchar](6) NULL,
	[Change_Status] [varchar](10) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [Shadow].[ReviewExemption]    Script Date: 4/15/2020 4:17:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Shadow].[ReviewExemption](
	[ID] [int] NOT NULL,
	[ProjectID] [varchar](6) NOT NULL,
	[StageID] [varchar](5) NOT NULL,
	[ExemptionType] [varchar](50) NOT NULL,
	[ExemptionReason] [varchar](1000) NULL,
	[ApproverComment] [varchar](200) NULL,
	[Approver] [varchar](6) NULL,
	[Approved] [varchar](1) NULL,
	[SubmissionComment] [varchar](200) NULL,
	[Requester] [varchar](6) NULL,
	[LastUpdated] [datetime] NULL,
	[UpdatedBy] [varchar](10) NULL,
	[Change_Status] [varchar](10) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [Shadow].[ReviewMaster]    Script Date: 4/15/2020 4:17:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Shadow].[ReviewMaster](
	[ProjectID] [varchar](6) NOT NULL,
	[ReviewID] [int] NOT NULL,
	[ReviewType] [varchar](255) NULL,
	[ReviewDate] [datetime] NULL,
	[DeferralDate] [datetime] NULL,
	[RiskScore] [varchar](10) NULL,
	[Status] [varchar](6) NULL,
	[Approved] [varchar](1) NULL,
	[LastUpdated] [datetime] NULL,
	[UserID] [varchar](6) NULL,
	[StageID] [varchar](5) NULL,
	[ApproveComment] [varchar](500) NULL,
	[Approver] [varchar](6) NULL,
	[SubmissionComment] [varchar](500) NULL,
	[Requester] [varchar](6) NULL,
	[ReviewScore] [decimal](18, 1) NULL,
	[Change_Status] [varchar](10) NULL,
	[DueDate] [datetime] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [Shadow].[ReviewOutputs]    Script Date: 4/15/2020 4:17:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Shadow].[ReviewOutputs](
	[ProjectID] [varchar](6) NOT NULL,
	[ReviewID] [int] NOT NULL,
	[OutputID] [int] NOT NULL,
	[OutputDescription] [varchar](500) NULL,
	[Weight] [int] NULL,
	[OutputScore] [varchar](3) NULL,
	[ImpactScore] [float] NULL,
	[Risk] [varchar](10) NULL,
	[Status] [varchar](6) NULL,
	[LastUpdated] [datetime] NULL,
	[UserID] [varchar](6) NULL,
	[Change_Status] [varchar](10) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [Shadow].[ReviewPCRScore]    Script Date: 4/15/2020 4:17:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Shadow].[ReviewPCRScore](
	[ProjectID] [varchar](6) NOT NULL,
	[ReviewID] [int] NOT NULL,
	[FinalOutputScore] [varchar](3) NULL,
	[OutcomeScore] [varchar](3) NULL,
	[ProgressToImpact] [varchar](1000) NULL,
	[CompletedToTimescales] [varchar](1) NULL,
	[CompletedToCost] [varchar](1) NULL,
	[FailedJustification] [varchar](1000) NULL,
	[Status] [varchar](6) NULL,
	[LastUpdated] [datetime] NULL,
	[UserID] [varchar](6) NULL,
	[Change_Status] [varchar](10) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [Shadow].[RiskDocument]    Script Date: 4/15/2020 4:17:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Shadow].[RiskDocument](
	[RiskRegisterID] [int] IDENTITY(1,1) NOT NULL,
	[ProjectID] [varchar](6) NOT NULL,
	[DocumentID] [varchar](12) NOT NULL,
	[Description] [varchar](max) NOT NULL,
	[LastUpdate] [datetime] NULL,
	[UserID] [varchar](6) NULL,
	[Change_State] [varchar](1) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [Shadow].[RiskDocumentTemp]    Script Date: 4/15/2020 4:17:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Shadow].[RiskDocumentTemp](
	[RiskRegisterID] [int] IDENTITY(1,1) NOT NULL,
	[ProjectID] [varchar](6) NOT NULL,
	[DocumentID] [varchar](7) NOT NULL,
	[Description] [varchar](max) NOT NULL,
	[LastUpdate] [datetime] NULL,
	[UserID] [varchar](6) NULL,
	[Change_State] [varchar](1) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [Shadow].[RiskRegister]    Script Date: 4/15/2020 4:17:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Shadow].[RiskRegister](
	[RiskID] [int] IDENTITY(1,1) NOT NULL,
	[ProjectID] [varchar](6) NOT NULL,
	[RiskDescription] [varchar](max) NULL,
	[Owner] [varchar](6) NOT NULL,
	[RiskCategory] [int] NOT NULL,
	[RiskLikelihood] [int] NULL,
	[RiskImpact] [int] NULL,
	[GrossRisk] [varchar](10) NOT NULL,
	[MitigationStrategy] [varchar](max) NULL,
	[ResidualLikelihood] [int] NULL,
	[ResidualImpact] [int] NULL,
	[ResidualRisk] [varchar](10) NOT NULL,
	[Comments] [varchar](max) NULL,
	[ExternalOwner] [varchar](max) NULL,
	[Status] [varchar](1) NOT NULL,
	[LastUpdated] [datetime] NOT NULL,
	[UserID] [varchar](6) NOT NULL,
	[Change_State] [varchar](1) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [Shadow].[Team]    Script Date: 4/15/2020 4:17:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Shadow].[Team](
	[Identity] [int] IDENTITY(1,1) NOT NULL,
	[Change_Status] [varchar](255) NOT NULL,
	[ProjectID] [varchar](6) NOT NULL,
	[TeamID] [varchar](255) NOT NULL,
	[RoleID] [varchar](255) NOT NULL,
	[Status] [varchar](255) NULL,
	[StartDate] [datetime] NOT NULL,
	[EndDate] [datetime] NULL,
	[LastUpdated] [datetime] NOT NULL,
	[UserID] [varchar](6) NULL,
PRIMARY KEY CLUSTERED 
(
	[Identity] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [Shadow].[WorkflowMaster]    Script Date: 4/15/2020 4:17:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Shadow].[WorkflowMaster](
	[Change_Status] [varchar](10) NULL,
	[WorkFlowID] [int] NOT NULL,
	[WorkFlowStepID] [int] NOT NULL,
	[TaskID] [int] NOT NULL,
	[StageID] [int] NOT NULL,
	[ProjectID] [varchar](6) NOT NULL,
	[ActionBy] [varchar](6) NULL,
	[ActionDate] [datetime] NULL,
	[Recipient] [varchar](6) NULL,
	[ActionComments] [varchar](500) NULL,
	[Status] [varchar](255) NULL,
	[LastUpdate] [datetime] NULL,
	[UserID] [varchar](6) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [System].[AdminUsers]    Script Date: 4/15/2020 4:17:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [System].[AdminUsers](
	[AdminUserID] [varchar](6) NOT NULL,
	[Status] [varchar](255) NOT NULL,
	[LastUpdated] [datetime] NOT NULL,
	[UserID] [varchar](6) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[AdminUserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [System].[BenefitingCountry]    Script Date: 4/15/2020 4:17:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [System].[BenefitingCountry](
	[BenefitingCountryID] [varchar](2) NOT NULL,
	[BenefitingCountryDescription] [varchar](255) NULL,
	[Status] [varchar](1) NULL,
	[LastUpdated] [datetime] NULL,
	[UserID] [varchar](6) NULL,
PRIMARY KEY CLUSTERED 
(
	[BenefitingCountryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [System].[BudgetCentre]    Script Date: 4/15/2020 4:17:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [System].[BudgetCentre](
	[BudgetCentreID] [varchar](5) NOT NULL,
	[BudgetCentreDescription] [varchar](255) NULL,
	[Status] [varchar](1) NULL,
	[LastUpdated] [datetime] NULL,
	[UserID] [varchar](6) NULL,
PRIMARY KEY CLUSTERED 
(
	[BudgetCentreID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [System].[CodePerformance]    Script Date: 4/15/2020 4:17:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [System].[CodePerformance](
	[LogID] [int] IDENTITY(1,1) NOT NULL,
	[MethodName] [varchar](255) NULL,
	[Description] [varchar](255) NULL,
	[Value] [varchar](6) NULL,
	[From] [datetime] NULL,
	[To] [datetime] NULL,
	[Result] [float] NULL,
	[LastUpdated] [datetime] NULL,
	[UserID] [varchar](6) NULL,
PRIMARY KEY CLUSTERED 
(
	[LogID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [System].[ComponentDateHashtable]    Script Date: 4/15/2020 4:17:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [System].[ComponentDateHashtable](
	[CheckSumValue] [int] NULL,
	[work_order] [varchar](25) NOT NULL,
	[date_from] [datetime] NOT NULL,
	[date_to] [datetime] NOT NULL,
	[start_date] [datetime] NULL,
	[end_date] [datetime] NULL,
	[last_update] [datetime] NULL,
	[UserID] [varchar](6) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [System].[ComponentDates]    Script Date: 4/15/2020 4:17:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [System].[ComponentDates](
	[CheckSumValue] [int] NULL,
	[work_order] [varchar](25) NOT NULL,
	[date_from] [datetime] NOT NULL,
	[date_to] [datetime] NOT NULL,
	[start_date] [datetime] NULL,
	[end_date] [datetime] NULL,
	[last_update] [datetime] NULL,
	[UserID] [varchar](6) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [System].[ComponentHashtable]    Script Date: 4/15/2020 4:17:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [System].[ComponentHashtable](
	[CheckSumValue] [int] NULL,
	[work_order] [varchar](25) NOT NULL,
	[description] [varchar](255) NOT NULL,
	[InputterID] [varchar](7) NULL,
	[QualityAssurerID] [varchar](7) NULL,
	[department] [varchar](25) NOT NULL,
	[project] [varchar](25) NOT NULL,
	[dfid_role] [varchar](25) NOT NULL,
	[admin_bud_approver] [varchar](25) NOT NULL,
	[funding_mechanism] [varchar](25) NOT NULL,
	[op_status] [varchar](25) NOT NULL,
	[benefitting_country] [varchar](25) NOT NULL,
	[last_update] [datetime] NOT NULL,
	[UserID] [varchar](6) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [System].[DateHashTable]    Script Date: 4/15/2020 4:17:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [System].[DateHashTable](
	[CheckSumValue] [int] NULL,
	[project] [varchar](25) NOT NULL,
	[created_date] [datetime] NOT NULL,
	[date_from] [datetime] NOT NULL,
	[date_to] [datetime] NOT NULL,
	[start_date] [datetime] NULL,
	[end_date] [datetime] NULL,
	[actual_start] [datetime] NULL,
	[promp_compl] [datetime] NULL,
	[ies_date] [datetime] NULL,
	[last_update] [datetime] NOT NULL,
	[User_ID] [varchar](25) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [System].[ErrorLog]    Script Date: 4/15/2020 4:17:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [System].[ErrorLog](
	[ErrorID] [int] IDENTITY(1,1) NOT NULL,
	[ProjectID] [varchar](10) NULL,
	[ErrorMessage] [varchar](max) NULL,
	[InnerException] [varchar](max) NULL,
	[StackTrace] [varchar](max) NULL,
	[CustomError] [varchar](max) NULL,
	[LastUpdated] [datetime] NULL,
	[UserID] [varchar](6) NULL,
PRIMARY KEY CLUSTERED 
(
	[ErrorID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [System].[EvaluationManagement]    Script Date: 4/15/2020 4:17:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [System].[EvaluationManagement](
	[EvaluationManagementID] [varchar](6) NOT NULL,
	[EvaluationManagementDescription] [varchar](255) NULL,
	[Status] [varchar](1) NULL,
	[LastUpdated] [datetime] NULL,
	[UserID] [varchar](6) NULL,
PRIMARY KEY CLUSTERED 
(
	[EvaluationManagementID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [System].[EvaluationType]    Script Date: 4/15/2020 4:17:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [System].[EvaluationType](
	[EvaluationTypeID] [varchar](6) NOT NULL,
	[EvaluationDescription] [varchar](255) NULL,
	[Status] [varchar](1) NULL,
	[LastUpdated] [datetime] NULL,
	[UserID] [varchar](6) NULL,
PRIMARY KEY CLUSTERED 
(
	[EvaluationTypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [System].[ExemptionReason]    Script Date: 4/15/2020 4:17:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [System].[ExemptionReason](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ExemptionID] [int] NULL,
	[ExemptionType] [varchar](3) NOT NULL,
	[ExemptionReason] [varchar](150) NULL,
	[Status] [varchar](1) NULL,
	[LastUpdated] [datetime] NULL,
	[UserID] [varchar](6) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [System].[FundingArrangement]    Script Date: 4/15/2020 4:17:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [System].[FundingArrangement](
	[FundingArrangementValue] [varchar](10) NOT NULL,
	[FundingArrangementType] [varchar](100) NULL,
	[Status] [varchar](1) NULL,
	[LastUpdated] [datetime] NULL,
	[UserID] [varchar](6) NULL,
PRIMARY KEY CLUSTERED 
(
	[FundingArrangementValue] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [System].[FundingMech]    Script Date: 4/15/2020 4:17:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [System].[FundingMech](
	[FundingMechID] [varchar](25) NOT NULL,
	[FundingMechDescription] [varchar](255) NULL,
	[Status] [varchar](1) NULL,
	[LastUpdated] [datetime] NULL,
	[UserID] [varchar](6) NULL,
PRIMARY KEY CLUSTERED 
(
	[FundingMechID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [System].[FundingMechToSector]    Script Date: 4/15/2020 4:17:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [System].[FundingMechToSector](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[SectorCode] [varchar](7) NULL,
	[FundingMech] [varchar](255) NULL,
	[Status] [varchar](6) NULL,
	[LastUpdated] [datetime] NULL,
	[UserID] [varchar](6) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [System].[InputSector]    Script Date: 4/15/2020 4:17:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [System].[InputSector](
	[InputSectorCodeID] [varchar](7) NOT NULL,
	[InputSectorCodeDescription] [varchar](255) NULL,
	[Status] [varchar](1) NULL,
	[LastUpdated] [datetime] NULL,
	[UserID] [varchar](6) NULL,
PRIMARY KEY CLUSTERED 
(
	[InputSectorCodeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [System].[Logging]    Script Date: 4/15/2020 4:17:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [System].[Logging](
	[LogID] [int] IDENTITY(1,1) NOT NULL,
	[ProjectID] [varchar](10) NULL,
	[ViewName] [varchar](255) NULL,
	[LastUpdated] [datetime] NULL,
	[UserID] [varchar](6) NULL,
PRIMARY KEY CLUSTERED 
(
	[LogID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [System].[PartnerOrganisation]    Script Date: 4/15/2020 4:17:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [System].[PartnerOrganisation](
	[PartnerOrganisationValue] [varchar](10) NOT NULL,
	[PartnerOrganisationType] [varchar](250) NULL,
	[Status] [varchar](1) NULL,
	[LastUpdated] [datetime] NULL,
	[UserID] [varchar](6) NULL,
PRIMARY KEY CLUSTERED 
(
	[PartnerOrganisationValue] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [System].[Portfolio]    Script Date: 4/15/2020 4:17:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [System].[Portfolio](
	[PortfolioID] [int] IDENTITY(1,1) NOT NULL,
	[ProjectID] [varchar](6) NULL,
	[Status] [varchar](10) NULL,
	[LastUpdated] [datetime] NULL,
	[UserID] [varchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[PortfolioID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [System].[ProjectHashtable]    Script Date: 4/15/2020 4:17:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [System].[ProjectHashtable](
	[CheckSumValue] [int] NULL,
	[project] [varchar](25) NOT NULL,
	[description] [varchar](255) NOT NULL,
	[Purpose] [varchar](8000) NULL,
	[department] [varchar](25) NOT NULL,
	[Stage] [varchar](25) NOT NULL,
	[Status] [varchar](25) NOT NULL,
	[last_update] [datetime] NOT NULL,
	[User] [varchar](6) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [System].[ProjectInfoTable]    Script Date: 4/15/2020 4:17:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [System].[ProjectInfoTable](
	[CheckSumValue] [int] NULL,
	[project] [varchar](25) NOT NULL,
	[ovis] [varchar](1000) NULL,
	[team_marker] [varchar](100) NULL,
	[approval_risk] [varchar](10) NULL,
	[specific_conditions] [varchar](25) NULL,
	[GenderEquality] [varchar](25) NULL,
	[HIVAIDS] [varchar](25) NULL,
	[last_update] [datetime] NOT NULL,
	[User_ID] [varchar](6) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [System].[ProjectRole]    Script Date: 4/15/2020 4:17:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [System].[ProjectRole](
	[ProjectRoleID] [varchar](255) NOT NULL,
	[ProjectRoleDescription] [varchar](255) NULL,
	[Status] [varchar](1) NULL,
	[LastUpdated] [datetime] NULL,
	[UserID] [varchar](6) NULL,
PRIMARY KEY CLUSTERED 
(
	[ProjectRoleID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [System].[ProjectTeamHash]    Script Date: 4/15/2020 4:17:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [System].[ProjectTeamHash](
	[CheckSumValue] [int] NULL,
	[ProjectID] [varchar](6) NULL,
	[name] [varchar](25) NOT NULL,
	[role] [varchar](25) NOT NULL,
	[Status] [varchar](1) NOT NULL,
	[date_from_fx] [datetime] NULL,
	[date_to_fx] [datetime] NULL,
	[last_update] [datetime] NULL,
	[User_ID] [varchar](25) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [System].[Risk]    Script Date: 4/15/2020 4:17:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [System].[Risk](
	[RiskValue] [varchar](10) NOT NULL,
	[RiskTitle] [varchar](25) NULL,
	[Status] [varchar](1) NULL,
	[LastUpdated] [datetime] NULL,
	[UserID] [varchar](6) NULL,
PRIMARY KEY CLUSTERED 
(
	[RiskValue] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [System].[RiskCategory]    Script Date: 4/15/2020 4:17:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [System].[RiskCategory](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[RiskCategoryDescription] [varchar](255) NULL,
	[Status] [varchar](1) NULL,
	[LastUpdated] [datetime] NULL,
	[UserID] [varchar](6) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [System].[RiskImpact]    Script Date: 4/15/2020 4:17:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [System].[RiskImpact](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[RiskImpactDescription] [varchar](255) NULL,
	[Status] [varchar](1) NULL,
	[LastUpdated] [datetime] NULL,
	[UserID] [varchar](6) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [System].[RiskLikelihood]    Script Date: 4/15/2020 4:17:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [System].[RiskLikelihood](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[RiskLikelihoodDescription] [varchar](255) NULL,
	[Status] [varchar](1) NULL,
	[LastUpdated] [datetime] NULL,
	[UserID] [varchar](6) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [System].[Stage]    Script Date: 4/15/2020 4:17:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [System].[Stage](
	[StageID] [varchar](5) NOT NULL,
	[StageDescription] [varchar](255) NULL,
	[Status] [varchar](1) NULL,
	[LastUpdated] [datetime] NULL,
	[UserID] [varchar](6) NULL,
PRIMARY KEY CLUSTERED 
(
	[StageID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [System].[UserLookUp]    Script Date: 4/15/2020 4:17:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [System].[UserLookUp](
	[ResourceID] [varchar](6) NOT NULL,
	[UserName] [varchar](max) NULL,
	[UserLogon] [varchar](max) NULL,
	[LastUpdated] [datetime] NULL,
	[UserID] [varchar](6) NULL,
PRIMARY KEY CLUSTERED 
(
	[ResourceID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [System].[Users]    Script Date: 4/15/2020 4:17:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [System].[Users](
	[UserID] [varchar](7) NOT NULL,
	[UserName] [varchar](1000) NULL,
	[Logon] [varchar](1000) NULL,
	[Status] [varchar](1) NULL,
	[LastUpdated] [datetime] NULL,
	[UserUpdated] [varchar](6) NULL,
PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [Workflow].[ProjectPlannedEndDates]    Script Date: 4/15/2020 4:17:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Workflow].[ProjectPlannedEndDates](
	[Identity] [int] IDENTITY(1,1) NOT NULL,
	[ProjectID] [varchar](6) NOT NULL,
	[CurrentPlannedEndDate] [datetime] NOT NULL,
	[NewPlannedEndDate] [datetime] NOT NULL,
	[Status] [varchar](255) NULL,
	[LastUpdate] [datetime] NULL,
	[UserID] [varchar](6) NULL,
	[WorkTaskID] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Identity] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [Workflow].[WorkflowDocuments]    Script Date: 4/15/2020 4:17:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Workflow].[WorkflowDocuments](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[WorkflowID] [int] NOT NULL,
	[DocumentID] [varchar](12) NOT NULL,
	[Description] [varchar](max) NOT NULL,
	[Status] [varchar](255) NULL,
	[LastUpdate] [datetime] NULL,
	[UserID] [varchar](6) NULL,
	[DocSource] [varchar](1) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [Workflow].[WorkflowDocumentsTemp]    Script Date: 4/15/2020 4:17:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Workflow].[WorkflowDocumentsTemp](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[WorkflowID] [int] NOT NULL,
	[DocumentID] [varchar](7) NOT NULL,
	[Description] [varchar](max) NOT NULL,
	[Status] [varchar](255) NULL,
	[LastUpdate] [datetime] NULL,
	[UserID] [varchar](6) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [Workflow].[WorkflowStage]    Script Date: 4/15/2020 4:17:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Workflow].[WorkflowStage](
	[StageID] [int] NOT NULL,
	[Description] [varchar](500) NOT NULL,
	[Status] [varchar](255) NULL,
	[LastUpdate] [datetime] NULL,
	[UserID] [varchar](6) NULL,
PRIMARY KEY CLUSTERED 
(
	[StageID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[TBL_Contacts] ADD  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[TBL_ContactTypes] ADD  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[TBL_ContractContent] ADD  DEFAULT ((0)) FOR [Value]
GO
ALTER TABLE [dbo].[TBL_ContractContent] ADD  DEFAULT ((0)) FOR [isExempt]
GO
ALTER TABLE [dbo].[TBL_EmailRules] ADD  DEFAULT ((0)) FOR [OffMonth]
GO
ALTER TABLE [dbo].[TBL_EmailRules] ADD  DEFAULT ((0)) FOR [OffWeek]
GO
ALTER TABLE [dbo].[TBL_EmailRules] ADD  DEFAULT ((0)) FOR [OffDay]
GO
ALTER TABLE [dbo].[TBL_EmailRules] ADD  DEFAULT ((0)) FOR [FreqMonth]
GO
ALTER TABLE [dbo].[TBL_EmailRules] ADD  DEFAULT ((0)) FOR [FreqWeek]
GO
ALTER TABLE [dbo].[TBL_EmailRules] ADD  DEFAULT ((0)) FOR [FreqDay]
GO
ALTER TABLE [dbo].[TBL_Project_Contacts] ADD  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[TBL_Projects] ADD  CONSTRAINT [DF_TBL_Projects_ProjectValue]  DEFAULT ((0)) FOR [ProjectValue]
GO
ALTER TABLE [dbo].[TBL_Projects] ADD  CONSTRAINT [DF_TBL_Projects_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[TBL_Terms] ADD  CONSTRAINT [DF__TBL_Terms__Perce__74794A92]  DEFAULT ((0)) FOR [Percentage]
GO
ALTER TABLE [dbo].[TBL_Terms_Country] ADD  CONSTRAINT [DF__TBL_Terms__Perce__756D6ECB]  DEFAULT ((0)) FOR [Percentage]
GO
ALTER TABLE [Component].[ComponentDates]  WITH CHECK ADD FOREIGN KEY([ComponentID])
REFERENCES [Component].[ComponentMaster] ([ComponentID])
GO
ALTER TABLE [Component].[ComponentMaster]  WITH CHECK ADD FOREIGN KEY([BenefittingCountry])
REFERENCES [System].[BenefitingCountry] ([BenefitingCountryID])
GO
ALTER TABLE [Component].[ComponentMaster]  WITH CHECK ADD FOREIGN KEY([BudgetCentreID])
REFERENCES [System].[BudgetCentre] ([BudgetCentreID])
GO
ALTER TABLE [Component].[ComponentMaster]  WITH CHECK ADD FOREIGN KEY([ProjectID])
REFERENCES [Project].[ProjectMaster] ([ProjectID])
GO
ALTER TABLE [Component].[DeliveryChain]  WITH CHECK ADD FOREIGN KEY([ComponentID])
REFERENCES [Component].[ComponentMaster] ([ComponentID])
GO
ALTER TABLE [Component].[ImplementingOrganisation]  WITH CHECK ADD FOREIGN KEY([ComponentID])
REFERENCES [Component].[ComponentMaster] ([ComponentID])
GO
ALTER TABLE [Component].[InputSectorCodes]  WITH CHECK ADD FOREIGN KEY([ComponentID])
REFERENCES [Component].[ComponentMaster] ([ComponentID])
GO
ALTER TABLE [Component].[InputSectorCodes]  WITH CHECK ADD FOREIGN KEY([InputSectorCode])
REFERENCES [System].[InputSector] ([InputSectorCodeID])
GO
ALTER TABLE [Component].[Markers]  WITH CHECK ADD FOREIGN KEY([ComponentID])
REFERENCES [Component].[ComponentMaster] ([ComponentID])
GO
ALTER TABLE [Component].[Markers]  WITH CHECK ADD FOREIGN KEY([ComponentID])
REFERENCES [Component].[ComponentMaster] ([ComponentID])
GO
ALTER TABLE [dbo].[TBL_Activity]  WITH CHECK ADD FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[TBL_Users] ([Id])
GO
ALTER TABLE [dbo].[Tbl_Applicants]  WITH NOCHECK ADD FOREIGN KEY([PqId])
REFERENCES [dbo].[TBL_Projects_PQ] ([Id])
GO
ALTER TABLE [dbo].[TBL_Contacts]  WITH CHECK ADD FOREIGN KEY([ContactTypeId])
REFERENCES [dbo].[TBL_ContactTypes] ([Id])
GO
ALTER TABLE [dbo].[TBL_Contacts]  WITH CHECK ADD FOREIGN KEY([CountryId])
REFERENCES [dbo].[TBL_Country] ([Id])
GO
ALTER TABLE [dbo].[TBL_ContractContent]  WITH CHECK ADD  CONSTRAINT [FK__TBL_Contr__Contr__558AAF1E] FOREIGN KEY([ContractId])
REFERENCES [dbo].[TBL_Contracts] ([Id])
GO
ALTER TABLE [dbo].[TBL_ContractContent] CHECK CONSTRAINT [FK__TBL_Contr__Contr__558AAF1E]
GO
ALTER TABLE [dbo].[TBL_ContractLC]  WITH CHECK ADD  CONSTRAINT [FK__TBL_Contr__Contr__4C0144E4] FOREIGN KEY([ContractId])
REFERENCES [dbo].[TBL_Contracts] ([Id])
GO
ALTER TABLE [dbo].[TBL_ContractLC] CHECK CONSTRAINT [FK__TBL_Contr__Contr__4C0144E4]
GO
ALTER TABLE [dbo].[TBL_ContractLocMap]  WITH CHECK ADD  CONSTRAINT [FK__TBL_Contr__Contr__4EDDB18F] FOREIGN KEY([ContractId])
REFERENCES [dbo].[TBL_Contracts] ([Id])
GO
ALTER TABLE [dbo].[TBL_ContractLocMap] CHECK CONSTRAINT [FK__TBL_Contr__Contr__4EDDB18F]
GO
ALTER TABLE [dbo].[TBL_ContractLocMap]  WITH CHECK ADD FOREIGN KEY([LocId])
REFERENCES [dbo].[TBL_LOC] ([Id])
GO
ALTER TABLE [dbo].[TBL_ContractResponsibility]  WITH CHECK ADD  CONSTRAINT [FK__TBL_Contr__Contr__4183B671] FOREIGN KEY([ContractId])
REFERENCES [dbo].[TBL_Contracts] ([Id])
GO
ALTER TABLE [dbo].[TBL_ContractResponsibility] CHECK CONSTRAINT [FK__TBL_Contr__Contr__4183B671]
GO
ALTER TABLE [dbo].[TBL_Contracts]  WITH CHECK ADD  CONSTRAINT [FK__TBL_Contra__PQId__01D345B0] FOREIGN KEY([PQId])
REFERENCES [dbo].[TBL_Projects_PQ] ([Id])
GO
ALTER TABLE [dbo].[TBL_Contracts] CHECK CONSTRAINT [FK__TBL_Contra__PQId__01D345B0]
GO
ALTER TABLE [dbo].[TBL_ContractTerms]  WITH CHECK ADD  CONSTRAINT [FK__TBL_Contr__Contr__52AE4273] FOREIGN KEY([ContractId])
REFERENCES [dbo].[TBL_Contracts] ([Id])
GO
ALTER TABLE [dbo].[TBL_ContractTerms] CHECK CONSTRAINT [FK__TBL_Contr__Contr__52AE4273]
GO
ALTER TABLE [dbo].[TBL_Country]  WITH CHECK ADD FOREIGN KEY([AddedBy])
REFERENCES [dbo].[TBL_Users] ([Id])
GO
ALTER TABLE [dbo].[TBL_Country]  WITH CHECK ADD FOREIGN KEY([RegionId])
REFERENCES [dbo].[TBL_Regions] ([Id])
GO
ALTER TABLE [dbo].[TBL_Country]  WITH CHECK ADD FOREIGN KEY([UpdatedBy])
REFERENCES [dbo].[TBL_Users] ([Id])
GO
ALTER TABLE [dbo].[TBL_LOC]  WITH CHECK ADD  CONSTRAINT [FK__TBL_LOC__Country__3DE82FB7] FOREIGN KEY([CountryId])
REFERENCES [dbo].[TBL_Country] ([Id])
GO
ALTER TABLE [dbo].[TBL_LOC] CHECK CONSTRAINT [FK__TBL_LOC__Country__3DE82FB7]
GO
ALTER TABLE [dbo].[TBL_LOC_Amendments]  WITH CHECK ADD FOREIGN KEY([LOC_Id])
REFERENCES [dbo].[TBL_LOC] ([Id])
GO
ALTER TABLE [dbo].[TBL_LOC_Contract]  WITH CHECK ADD  CONSTRAINT [FK__TBL_LOC_C__Contr__04AFB25B] FOREIGN KEY([ContractId])
REFERENCES [dbo].[TBL_Contracts] ([Id])
GO
ALTER TABLE [dbo].[TBL_LOC_Contract] CHECK CONSTRAINT [FK__TBL_LOC_C__Contr__04AFB25B]
GO
ALTER TABLE [dbo].[TBL_LOC_Contract]  WITH CHECK ADD  CONSTRAINT [FK__TBL_LOC_C__LocId__43A1090D] FOREIGN KEY([LocId])
REFERENCES [dbo].[TBL_LOC] ([Id])
GO
ALTER TABLE [dbo].[TBL_LOC_Contract] CHECK CONSTRAINT [FK__TBL_LOC_C__LocId__43A1090D]
GO
ALTER TABLE [dbo].[TBL_LOC_Project]  WITH CHECK ADD  CONSTRAINT [FK__TBL_LOC_P__LocId__477199F1] FOREIGN KEY([LocId])
REFERENCES [dbo].[TBL_LOC] ([Id])
GO
ALTER TABLE [dbo].[TBL_LOC_Project] CHECK CONSTRAINT [FK__TBL_LOC_P__LocId__477199F1]
GO
ALTER TABLE [dbo].[TBL_LOC_Project]  WITH CHECK ADD  CONSTRAINT [FK__TBL_LOC_P__Proje__4865BE2A] FOREIGN KEY([ProjectId])
REFERENCES [dbo].[TBL_Projects] ([Id])
GO
ALTER TABLE [dbo].[TBL_LOC_Project] CHECK CONSTRAINT [FK__TBL_LOC_P__Proje__4865BE2A]
GO
ALTER TABLE [dbo].[TBL_LocBalance]  WITH CHECK ADD FOREIGN KEY([LocId])
REFERENCES [dbo].[TBL_LOC] ([Id])
GO
ALTER TABLE [dbo].[TBL_Options]  WITH CHECK ADD FOREIGN KEY([AddedBy])
REFERENCES [dbo].[TBL_Users] ([Id])
GO
ALTER TABLE [dbo].[TBL_Options]  WITH CHECK ADD FOREIGN KEY([UpdatedBy])
REFERENCES [dbo].[TBL_Users] ([Id])
GO
ALTER TABLE [dbo].[TBL_Project_Contacts]  WITH CHECK ADD FOREIGN KEY([ContactId])
REFERENCES [dbo].[TBL_Contacts] ([Id])
GO
ALTER TABLE [dbo].[TBL_Project_Contacts]  WITH CHECK ADD FOREIGN KEY([ProjectId])
REFERENCES [dbo].[TBL_Projects] ([Id])
GO
ALTER TABLE [dbo].[TBL_ProjectLocations]  WITH CHECK ADD FOREIGN KEY([ProjectId])
REFERENCES [dbo].[TBL_Projects] ([Id])
GO
ALTER TABLE [dbo].[TBL_ProjectLogs]  WITH CHECK ADD FOREIGN KEY([ProjectId])
REFERENCES [dbo].[TBL_Projects] ([Id])
GO
ALTER TABLE [dbo].[TBL_Projects]  WITH CHECK ADD  CONSTRAINT [FK_TBL_Status_Status] FOREIGN KEY([Status])
REFERENCES [dbo].[TBL_Status] ([Id])
GO
ALTER TABLE [dbo].[TBL_Projects] CHECK CONSTRAINT [FK_TBL_Status_Status]
GO
ALTER TABLE [dbo].[TBL_Projects_PQ]  WITH CHECK ADD  CONSTRAINT [FK__TBL_Proje__Proje__308E3499] FOREIGN KEY([ProjectId])
REFERENCES [dbo].[TBL_Projects] ([Id])
GO
ALTER TABLE [dbo].[TBL_Projects_PQ] CHECK CONSTRAINT [FK__TBL_Proje__Proje__308E3499]
GO
ALTER TABLE [dbo].[TBL_ProjectTimeLines]  WITH CHECK ADD FOREIGN KEY([ProjectId])
REFERENCES [dbo].[TBL_Projects] ([Id])
GO
ALTER TABLE [dbo].[TBL_Regions]  WITH CHECK ADD FOREIGN KEY([AddedBy])
REFERENCES [dbo].[TBL_Users] ([Id])
GO
ALTER TABLE [dbo].[TBL_Regions]  WITH CHECK ADD FOREIGN KEY([UpdatedBy])
REFERENCES [dbo].[TBL_Users] ([Id])
GO
ALTER TABLE [dbo].[TBL_RuleTransactions]  WITH CHECK ADD FOREIGN KEY([RuleId])
REFERENCES [dbo].[TBL_EmailRules] ([Id])
GO
ALTER TABLE [dbo].[TBL_StakeHolders]  WITH CHECK ADD  CONSTRAINT [FK__TBL_Stake__Proje__345EC57D] FOREIGN KEY([ProjectId])
REFERENCES [dbo].[TBL_Projects] ([Id])
GO
ALTER TABLE [dbo].[TBL_StakeHolders] CHECK CONSTRAINT [FK__TBL_Stake__Proje__345EC57D]
GO
ALTER TABLE [dbo].[TBL_StakeHolders]  WITH CHECK ADD FOREIGN KEY([UserId])
REFERENCES [System].[Users] ([UserID])
GO
ALTER TABLE [dbo].[TBl_TeamMapping]  WITH CHECK ADD FOREIGN KEY([UserId])
REFERENCES [dbo].[TBL_Users] ([Id])
GO
ALTER TABLE [dbo].[TBL_Terms]  WITH CHECK ADD FOREIGN KEY([AddedBy])
REFERENCES [dbo].[TBL_Users] ([Id])
GO
ALTER TABLE [dbo].[TBL_Terms]  WITH CHECK ADD FOREIGN KEY([UpdatedBy])
REFERENCES [dbo].[TBL_Users] ([Id])
GO
ALTER TABLE [dbo].[TBL_Terms_Country]  WITH CHECK ADD  CONSTRAINT [FK__TBL_Terms__Count__11158940] FOREIGN KEY([CountryId])
REFERENCES [dbo].[TBL_Country] ([Id])
GO
ALTER TABLE [dbo].[TBL_Terms_Country] CHECK CONSTRAINT [FK__TBL_Terms__Count__11158940]
GO
ALTER TABLE [dbo].[TBL_UserRoleMap]  WITH CHECK ADD FOREIGN KEY([RoleId])
REFERENCES [dbo].[TBL_Roles] ([Id])
GO
ALTER TABLE [dbo].[TBL_UserRoleMap]  WITH CHECK ADD FOREIGN KEY([UserId])
REFERENCES [dbo].[TBL_Users] ([Id])
GO
ALTER TABLE [dbo].[TBL_Users]  WITH CHECK ADD FOREIGN KEY([ContactId])
REFERENCES [dbo].[TBL_Contacts] ([Id])
GO
ALTER TABLE [Location].[GeoCodes]  WITH CHECK ADD FOREIGN KEY([CountryCodeID])
REFERENCES [Location].[CountryCodes] ([CountryCodeID])
GO
ALTER TABLE [Location].[GeoCodes]  WITH CHECK ADD FOREIGN KEY([LocationTypeID])
REFERENCES [Location].[LocationType] ([LocationTypeID])
GO
ALTER TABLE [Location].[GeoCodes]  WITH CHECK ADD FOREIGN KEY([PrecisionID])
REFERENCES [Location].[Precision] ([PrecisionID])
GO
ALTER TABLE [Location].[GeoCodes]  WITH CHECK ADD FOREIGN KEY([ProjectID])
REFERENCES [Project].[ProjectMaster] ([ProjectID])
GO
ALTER TABLE [Location].[GeoCodes]  WITH CHECK ADD FOREIGN KEY([ProjectID])
REFERENCES [Project].[ProjectMaster] ([ProjectID])
GO
ALTER TABLE [Project].[AuditedFinancialStatements]  WITH CHECK ADD FOREIGN KEY([ProjectID])
REFERENCES [Project].[ProjectMaster] ([ProjectID])
GO
ALTER TABLE [Project].[ConditionalityReview]  WITH CHECK ADD FOREIGN KEY([ProjectID])
REFERENCES [Project].[ProjectMaster] ([ProjectID])
GO
ALTER TABLE [Project].[Deferral]  WITH CHECK ADD FOREIGN KEY([ProjectID])
REFERENCES [Project].[ProjectMaster] ([ProjectID])
GO
ALTER TABLE [Project].[Deferral]  WITH CHECK ADD FOREIGN KEY([ProjectID])
REFERENCES [Project].[ProjectMaster] ([ProjectID])
GO
ALTER TABLE [Project].[DSOMarkers]  WITH CHECK ADD FOREIGN KEY([ProjectID])
REFERENCES [Project].[ProjectMaster] ([ProjectID])
GO
ALTER TABLE [Project].[Evaluation]  WITH CHECK ADD FOREIGN KEY([EvaluationTypeID])
REFERENCES [System].[EvaluationType] ([EvaluationTypeID])
GO
ALTER TABLE [Project].[Evaluation]  WITH CHECK ADD FOREIGN KEY([ManagementOfEvaluation])
REFERENCES [System].[EvaluationManagement] ([EvaluationManagementID])
GO
ALTER TABLE [Project].[Evaluation]  WITH CHECK ADD FOREIGN KEY([ProjectID])
REFERENCES [Project].[ProjectMaster] ([ProjectID])
GO
ALTER TABLE [Project].[EvaluationDocuments]  WITH CHECK ADD FOREIGN KEY([EvaluationID])
REFERENCES [Project].[Evaluation] ([EvaluationID])
GO
ALTER TABLE [Project].[Markers]  WITH CHECK ADD FOREIGN KEY([ProjectID])
REFERENCES [Project].[ProjectMaster] ([ProjectID])
GO
ALTER TABLE [Project].[OverallRiskRating]  WITH CHECK ADD  CONSTRAINT [FK__OverallRi__RiskS__5804F8DB] FOREIGN KEY([RiskScore])
REFERENCES [System].[Risk] ([RiskValue])
GO
ALTER TABLE [Project].[OverallRiskRating] CHECK CONSTRAINT [FK__OverallRi__RiskS__5804F8DB]
GO
ALTER TABLE [Project].[Performance]  WITH CHECK ADD FOREIGN KEY([ProjectID])
REFERENCES [Project].[ProjectMaster] ([ProjectID])
GO
ALTER TABLE [Project].[ProjectBudgetCentreOrgUnit]  WITH CHECK ADD FOREIGN KEY([ProjectID])
REFERENCES [Project].[ProjectMaster] ([ProjectID])
GO
ALTER TABLE [Project].[ProjectDates]  WITH CHECK ADD FOREIGN KEY([ProjectID])
REFERENCES [Project].[ProjectMaster] ([ProjectID])
GO
ALTER TABLE [Project].[ProjectInfo]  WITH CHECK ADD FOREIGN KEY([ProjectID])
REFERENCES [Project].[ProjectMaster] ([ProjectID])
GO
ALTER TABLE [Project].[ProjectMaster]  WITH CHECK ADD FOREIGN KEY([BudgetCentreID])
REFERENCES [System].[BudgetCentre] ([BudgetCentreID])
GO
ALTER TABLE [Project].[ProjectMaster]  WITH CHECK ADD FOREIGN KEY([Stage])
REFERENCES [System].[Stage] ([StageID])
GO
ALTER TABLE [Project].[Reports]  WITH CHECK ADD FOREIGN KEY([ProjectID])
REFERENCES [Project].[ProjectMaster] ([ProjectID])
GO
ALTER TABLE [Project].[ReviewARScore]  WITH CHECK ADD FOREIGN KEY([ProjectID])
REFERENCES [Project].[ProjectMaster] ([ProjectID])
GO
ALTER TABLE [Project].[ReviewARScore]  WITH CHECK ADD FOREIGN KEY([ProjectID], [ReviewID])
REFERENCES [Project].[ReviewMaster] ([ProjectID], [ReviewID])
GO
ALTER TABLE [Project].[ReviewDeferral]  WITH CHECK ADD FOREIGN KEY([ProjectID])
REFERENCES [Project].[ProjectMaster] ([ProjectID])
GO
ALTER TABLE [Project].[ReviewDeferral]  WITH CHECK ADD  CONSTRAINT [FK__ReviewDef__Proje__73B00EE2] FOREIGN KEY([ProjectID])
REFERENCES [Project].[ProjectMaster] ([ProjectID])
GO
ALTER TABLE [Project].[ReviewDeferral] CHECK CONSTRAINT [FK__ReviewDef__Proje__73B00EE2]
GO
ALTER TABLE [Project].[ReviewDeferral]  WITH CHECK ADD FOREIGN KEY([StageID])
REFERENCES [Project].[ReviewStage] ([StageID])
GO
ALTER TABLE [Project].[ReviewDeferral]  WITH CHECK ADD  CONSTRAINT [FK__ReviewDef__Stage__72BBEAA9] FOREIGN KEY([StageID])
REFERENCES [Project].[ReviewStage] ([StageID])
GO
ALTER TABLE [Project].[ReviewDeferral] CHECK CONSTRAINT [FK__ReviewDef__Stage__72BBEAA9]
GO
ALTER TABLE [Project].[ReviewDocuments]  WITH CHECK ADD FOREIGN KEY([ProjectID], [ReviewID])
REFERENCES [Project].[ReviewMaster] ([ProjectID], [ReviewID])
GO
ALTER TABLE [Project].[ReviewExemption]  WITH CHECK ADD FOREIGN KEY([ProjectID])
REFERENCES [Project].[ProjectMaster] ([ProjectID])
GO
ALTER TABLE [Project].[ReviewExemption]  WITH CHECK ADD FOREIGN KEY([StageID])
REFERENCES [Project].[ReviewStage] ([StageID])
GO
ALTER TABLE [Project].[ReviewMaster]  WITH CHECK ADD FOREIGN KEY([ProjectID])
REFERENCES [Project].[ProjectMaster] ([ProjectID])
GO
ALTER TABLE [Project].[ReviewMaster]  WITH CHECK ADD FOREIGN KEY([RiskScore])
REFERENCES [System].[Risk] ([RiskValue])
GO
ALTER TABLE [Project].[ReviewMaster]  WITH CHECK ADD FOREIGN KEY([StageID])
REFERENCES [Project].[ReviewStage] ([StageID])
GO
ALTER TABLE [Project].[ReviewOutputs]  WITH CHECK ADD FOREIGN KEY([OutputScore])
REFERENCES [Project].[ReviewScore] ([OutputScore])
GO
ALTER TABLE [Project].[ReviewOutputs]  WITH CHECK ADD FOREIGN KEY([ProjectID])
REFERENCES [Project].[ProjectMaster] ([ProjectID])
GO
ALTER TABLE [Project].[ReviewOutputs]  WITH CHECK ADD FOREIGN KEY([ProjectID], [ReviewID])
REFERENCES [Project].[ReviewMaster] ([ProjectID], [ReviewID])
GO
ALTER TABLE [Project].[ReviewPCRScore]  WITH CHECK ADD FOREIGN KEY([ProjectID])
REFERENCES [Project].[ProjectMaster] ([ProjectID])
GO
ALTER TABLE [Project].[ReviewPCRScore]  WITH CHECK ADD FOREIGN KEY([ProjectID], [ReviewID])
REFERENCES [Project].[ReviewMaster] ([ProjectID], [ReviewID])
GO
ALTER TABLE [Project].[RiskDocument]  WITH CHECK ADD FOREIGN KEY([ProjectID])
REFERENCES [Project].[ProjectMaster] ([ProjectID])
GO
ALTER TABLE [Project].[RiskRegister]  WITH CHECK ADD FOREIGN KEY([GrossRisk])
REFERENCES [System].[Risk] ([RiskValue])
GO
ALTER TABLE [Project].[RiskRegister]  WITH CHECK ADD FOREIGN KEY([ProjectID])
REFERENCES [Project].[ProjectMaster] ([ProjectID])
GO
ALTER TABLE [Project].[RiskRegister]  WITH CHECK ADD FOREIGN KEY([ResidualRisk])
REFERENCES [System].[Risk] ([RiskValue])
GO
ALTER TABLE [Project].[RiskRegister]  WITH CHECK ADD FOREIGN KEY([RiskCategory])
REFERENCES [System].[RiskCategory] ([ID])
GO
ALTER TABLE [Project].[Team]  WITH CHECK ADD FOREIGN KEY([ProjectID])
REFERENCES [Project].[ProjectMaster] ([ProjectID])
GO
ALTER TABLE [Project].[Team]  WITH CHECK ADD FOREIGN KEY([RoleID])
REFERENCES [System].[ProjectRole] ([ProjectRoleID])
GO
ALTER TABLE [Project].[Team]  WITH CHECK ADD FOREIGN KEY([RoleID])
REFERENCES [System].[ProjectRole] ([ProjectRoleID])
GO
ALTER TABLE [Project].[TeamExternal]  WITH CHECK ADD FOREIGN KEY([ProjectID])
REFERENCES [Project].[ProjectMaster] ([ProjectID])
GO
ALTER TABLE [Results].[OutputIndicator]  WITH CHECK ADD FOREIGN KEY([ProjectOutputID])
REFERENCES [Results].[ProjectOutput] ([ID])
GO
ALTER TABLE [Results].[OutputIndicatorMilestones]  WITH CHECK ADD FOREIGN KEY([OutputIndicatorID])
REFERENCES [Results].[OutputIndicator] ([ID])
GO
ALTER TABLE [Results].[ProjectOutput]  WITH CHECK ADD FOREIGN KEY([ProjectID])
REFERENCES [Project].[ProjectMaster] ([ProjectID])
GO
ALTER TABLE [System].[FundingMechToSector]  WITH CHECK ADD FOREIGN KEY([SectorCode])
REFERENCES [System].[InputSector] ([InputSectorCodeID])
GO
ALTER TABLE [System].[Portfolio]  WITH CHECK ADD FOREIGN KEY([ProjectID])
REFERENCES [Project].[ProjectMaster] ([ProjectID])
GO
ALTER TABLE [Workflow].[WorkflowMaster]  WITH CHECK ADD FOREIGN KEY([ProjectID])
REFERENCES [Project].[ProjectMaster] ([ProjectID])
GO
ALTER TABLE [Workflow].[WorkflowMaster]  WITH CHECK ADD FOREIGN KEY([StageID])
REFERENCES [Workflow].[WorkflowStage] ([StageID])
GO
ALTER TABLE [Workflow].[WorkflowMaster]  WITH CHECK ADD FOREIGN KEY([TaskID])
REFERENCES [Workflow].[WorkflowTask] ([TaskID])
GO
/****** Object:  StoredProcedure [dbo].[Add_ProjectToLOC]    Script Date: 4/15/2020 4:17:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create procedure [dbo].[Add_ProjectToLOC]
@allocation money,
@locid int,
@projectid int

as
begin
	set nocount on
	declare @msg varchar(max);
	declare @status bit;
	declare @processedId int;
	begin try
		insert into TBL_LOC_Project(LocId, ProjectId, Allocation) values(@locid, @projectid, @allocation);
		set @processedId = SCOPE_IDENTITY();
		set @status = 1;
	end try
	begin catch
		set @processedId = 0;
		set @msg = ERROR_MESSAGE();
		set @status = 0;
	end catch
	select @processedId as ProcessedId, @msg as Message, @status as Status

end;
GO
/****** Object:  StoredProcedure [dbo].[AddFiles]    Script Date: 4/15/2020 4:17:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Procedure [dbo].[AddFiles]
@src nvarchar(max),
@fileFor varchar(100),
@recordId int,
@displayname varchar(100),
@userid int
as
begin
	SET NOCOUNT ON
	declare @msg varchar(max);
	declare @_status bit;
	declare @processedId int;

	begin try
		
		insert into TBL_Files(Src, FileFor, RecordId, DisplayName)
		values (@src, @fileFor, @recordId, @displayname);

		insert into TBL_Activity(EntityId, EntityName, [Message], CreatedBy, CreatedOn)
		select @recordId, 'TBL_'+@fileFor, 'File '+@displayname +' added to '+ @fileFor, @userid, getdate()

		set @processedId = SCOPE_IDENTITY();
		set @msg = 'File Saved';
		set @_status = 1;

	end try
	begin catch
		set @processedId = 0;
		set @msg = ERROR_MESSAGE();
		set @_status = 0;
	end catch

	select @processedId as ProcessedId, @msg as Message, @_status as Status
end 


/****** Object:  StoredProcedure [dbo].[CreateUpdate_Loc]    Script Date: 7/30/2019 11:45:59 AM ******/
SET ANSI_NULLS ON
GO
/****** Object:  StoredProcedure [dbo].[AddProject]    Script Date: 4/15/2020 4:17:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE Procedure [dbo].[AddProject]
@id int,
@name varchar(300),
@description varchar(max),
@status int,
@dprDate datetime,
@baselineData varchar(max),
@locationAddress varchar(300),
@locationCordinates varchar(50),
@stage int,
@subSector int,
@preQualification bit,
@authority varchar(max),
@progress float,
@financialprogress float,
@projectValue decimal(18,2),
@projectStart datetime,
@projectEnd datetime,
@sector varchar(200),
@subsectorname varchar(MAX),
@address varchar(MAX),
@note varchar(max),
@physicalprogress float
as
begin
	SET NOCOUNT ON

	declare @msg varchar(max);
	declare @_status bit;
	declare @processedId int;
	declare @code varchar(max);
	begin try
		insert into TBL_Projects(Name, Description, Status, DPRDate, BaselineData, LocationAddress, LocationCoordinates, 
		Stage, SubSector, PreQualification, AuthorityName, Progress, FinancialProgress, ProjectValue,
		ProjectStart, ProjectEnd, CrOn, ModOn, IsActive, Sector, SubSectorName, [Address], Note)
		values(@name, @description, @status, @dprDate, @baselineData, @locationAddress, @locationCordinates, 
		@stage, @subSector, @preQualification, @authority, @progress,@financialprogress, 
		@projectValue, @projectStart, @projectEnd, GETDATE(), GETDATE(), 1, @sector, @subsectorname, @address, @note);

		set @processedId = SCOPE_IDENTITY();


		--adding projectcode
		set @code = 'P-' + RIGHT('0000000' + CONVERT(varchar, @processedId), 8);
		update TBL_Projects SET Code = @code, 
		Progress = (case when ProjectStart is null then 0 
						 when ProjectEnd is null then 0 
						 when datediff(day, ProjectStart, ProjectEnd) < datediff(day, ProjectStart, getdate()) then 100
						 else ceiling(datediff(day, ProjectStart, getdate())*1.00/datediff(day, ProjectStart, ProjectEnd) * 100) end)
		where Id= @processedId;


		
		if exists (select * from TBL_ProjectTimeLines where TimelineTitle = 'Project Start' and ProjectId = @processedId)
		begin
			update TBL_ProjectTimeLines set TimelineDate = @projectStart where TimelineTitle = 'Project Start' and ProjectId = @processedId
		end
		else
		begin
			insert into TBL_ProjectTimeLines (ProjectId, TimelineTitle, TimelineDate) select @processedId, 'Project Start', @projectStart
		end

		if exists (select * from TBL_ProjectTimeLines where TimelineTitle = 'Project End' and ProjectId = @processedId)
		begin
			update TBL_ProjectTimeLines set TimelineDate = @projectEnd where TimelineTitle = 'Project End' and ProjectId = @processedId
		end
		else
		begin
			insert into TBL_ProjectTimeLines (ProjectId, TimelineTitle, TimelineDate) select @processedId, 'Project End', @projectEnd
		end


		set @msg = '';
		set @_status = 1;

	end try
	begin catch
		set @processedId = 0;
		set @msg = ERROR_MESSAGE();
		set @_status = 0;
	end catch

	select @processedId as ProcessedId, @msg as Message, @_status as Status

end;
GO
/****** Object:  StoredProcedure [dbo].[AddUpdate_Contract]    Script Date: 4/15/2020 4:17:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE Procedure [dbo].[AddUpdate_Contract]
@Id int,
@ContractName nvarchar(255),
@TypeOfPackage nvarchar(255),
@EstimateValue decimal(16,6),
@ContractorCIF varchar(max),
@CGS varchar(max),
@PqId int,
@start datetime,
@end datetime,
@note varchar(max),
@scheduledDate datetime,
@ContractApprovalDate DATETIME,
@RevisedCompletionDate DATETIME,
@ActualCompletionDate DATETIME,
@TerminalDateOfDisbursement DATETIME,
@DateOfReceiptOfContractByEximBank DATETIME,
@SiginingDate DATETIME,
@SignEffectiveDate DATETIME,
@DurationYear INT,
@DurationMonth INT,
@DurationDay INT
as
Begin
                SET NOCOUNT ON
				Declare @ProjectId int;
                declare @msg varchar(max);
                declare @_status bit;
                declare @processedId int;
  Declare @NoCount int;
  declare @ContractId nvarchar(255), @projectcode nvarchar(255), @contract_cnt int;
  begin try
  begin transaction TR;
				Select @ProjectId = ProjectId from TBL_Projects_PQ where Id = @PqId;
                Select @NoCount = Count(*) from TBL_Contracts where Id = @Id;
                  if @NoCount = 0 begin
								Select @projectcode = Code from TBL_Projects where Id = @ProjectId;
								Select @contract_cnt = count(*)
								from TBL_Projects_PQ as a inner join TBL_Contracts as b
								on a.Id = b.PQId where a.ProjectId = @projectId; 
								SET @contract_cnt = (ISNULL(@contract_cnt, 0) + 1);
                                
                                INSERT into TBL_Contracts(PackageName, ContractorName, EstimateValue, TypeOfPackage,ContractType,ContractorCIF,CGSId ,PQId, ContractStart, 
								ContractEnd, Note, ScheduledCompDate,ContractApprovalDate,RevisedCompletionDate,ActualCompletionDate,TerminalDateOfDisbursement,
								DateOfReceiptOfContractByEximBank, SigningDate, SignEffectiveDate,DurationYear,DurationMonth,DurationDay) 
                                values(@ContractName,@ContractName, @EstimateValue, @TypeOfPackage,@TypeOfPackage, @ContractorCIF,UPPER(@CGS) ,@pqId, @start, @end, @note, 
								@scheduledDate,@ContractApprovalDate,@RevisedCompletionDate,@ActualCompletionDate,@TerminalDateOfDisbursement,
								@DateOfReceiptOfContractByEximBank, @SiginingDate, @SignEffectiveDate,@DurationYear,@DurationMonth,@DurationDay);
                                set @Id = SCOPE_IDENTITY();
								if @projectcode is not null begin
									set @ContractId = @projectcode + '_C_' + Convert(Varchar, @contract_cnt);
								end
								else begin
									set @ContractId = 'Contract-'+ Convert(Varchar, @Id);
								end
                                Update TBL_Contracts set PackageId = @ContractId WHERE Id = @Id;
                                set @processedId = @Id;
                                set @msg = 'Contract Added Successfully';
                                set @_status = 1;
                  end
                  else begin
                                Update TBL_Contracts set PackageName = @ContractName, ContractorName = @ContractName, EstimateValue = @EstimateValue, TypeOfPackage = @TypeOfPackage, 
								ContractType = @TypeOfPackage, ContractorCIF = @ContractorCIF, CGSId = UPPER(@CGS), ContractStart = @start, 
								ContractEnd = @end, Note = @note, 
								ScheduledCompDate = dateadd(day,@DurationDay,dateadd(month,@DurationMonth,dateadd(year,@DurationYear,@SignEffectiveDate))),
								ContractApprovalDate=@ContractApprovalDate,
								RevisedCompletionDate=@RevisedCompletionDate,ActualCompletionDate=@ActualCompletionDate,
								TerminalDateOfDisbursement=@TerminalDateOfDisbursement,DateOfReceiptOfContractByEximBank=@DateOfReceiptOfContractByEximBank,
								SigningDate = @SiginingDate, SignEffectiveDate = @SignEffectiveDate,DurationYear=@DurationYear,DurationMonth=@DurationMonth,DurationDay=@DurationDay
                                where Id = @Id;
                                set @processedId = @Id;
                                set @msg = 'Contract Updated Successfully';
                                set @_status = 1;
                  end


				  
				  --declare @projid int = (select ProjectId from TBL_Projects_PQ where Id in (
				  --select PQId from TBL_Contracts where Id = @processedId))
				  update TBL_Projects set FinancialProgress = (
				  select ceiling(sum(TA)/ sum(SA) *100) from 
				  (select CR.Id,max(SanctionedAmount) SA, sum(TranAmount) TA from TBL_Contracts  CR inner join Finacle_Contract_Transanctions CT on rtrim(ltrim(CR.CGSId)) = rtrim(ltrim(CT.AccountId)) 
				  where PQId in (select Id from TBL_Projects_PQ where ProjectId = @ProjectId) group by CR.Id) A
				  ) ,
				  DisbursedAmount = (
					select ceiling(sum(TA)) from 
				  (select CR.Id,max(SanctionedAmount) SA, sum(TranAmount) TA from TBL_Contracts  CR inner join Finacle_Contract_Transanctions CT on rtrim(ltrim(CR.CGSId)) = rtrim(ltrim(CT.AccountId)) 
				  where PQId in (select Id from TBL_Projects_PQ where ProjectId = @ProjectId) group by CR.Id) A
				  )				  
				  where Id = @ProjectId 

                  Commit TRansaction TR;
  end try
  begin catch
                set @processedId = 0;
                set @msg = ERROR_MESSAGE();
                set @_status = 0;
                Rollback Transaction TR;
  end catch
  select @processedId as ProcessedId, @msg as Message, @_status as Status
end;
GO
/****** Object:  StoredProcedure [dbo].[AddUpdate_ProjectTimelines]    Script Date: 4/15/2020 4:17:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create procedure [dbo].[AddUpdate_ProjectTimelines]
@id int,
@projectId int,
@timelineTitle varchar(50),
@timelineDate datetime
as
begin
declare @msg varchar(max);
declare @_status bit;
declare @processedId int;

BEGIN TRY
	if @id = 0 begin
		insert into TBL_ProjectTimeLines(ProjectId, TimelineTitle, TimelineDate)
		values(@projectId, @timelineTitle, @timelineDate);
		set @_status = 1;
		set @msg = 'Timeline saved';
		set @processedId = SCOPE_IDENTITY();
	end
	else begin
		Update TBL_ProjectTimeLines set TimelineTitle = @timelineTitle, 
		TimelineDate = @timelineDate where Id =@id;
		set @_status = 1;
		set @msg = 'Timeline saved';
		set @processedId = @id;
	end
END try
BEGIN CATCH
	set @processedId = 0;
	set @msg = ERROR_MESSAGE();
	set @_status = 0;
END catch
	select @processedId as ProcessedId, @msg as Message, @_status as Status
end

GO
/****** Object:  StoredProcedure [dbo].[AddUpdateContact]    Script Date: 4/15/2020 4:17:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create procedure [dbo].[AddUpdateContact]
@contactId int,
@name nvarchar(50),
@landline nvarchar(15),
@mobile nvarchar(15),
@fax nvarchar(15),
@email nvarchar(15),
@addrline1 nvarchar(15),
@addrline2 nvarchar(15),
@city nvarchar(15),
@pincode nvarchar(15),
@countryId int,
@contactTypeId int,
@organization nvarchar(15),
@designation nvarchar(15)
as
begin
	declare @msg varchar(max);
	declare @_status bit;
	declare @processedId int;
	begin try
		if @contactId = 0 begin
			insert into TBL_Contacts (Name, Landline, MobileNumber, Faxno, Email,
			AddressLine1, AddressLine2, City, PinCode, CountryId, ContactTypeId, 
			Organization, Designation, IsActive) values(@name, @landline, @mobile, @fax, @email,
			@addrline1, @addrline2, @city, @pincode, @countryId, @contactTypeId, 
			@organization, @designation, 1);

			set @processedId = SCOPE_IDENTITY();
			set @_status = 1;
			set @msg = 'Contact Detail Saved';
		end
		else begin
			Update  TBL_Contacts set Name=@name, Landline = @landline, MobileNumber = @mobile,
			Faxno = @fax, Email = @email, AddressLine1 = @addrline1, AddressLine2 = @addrline2,
			City = @city, PinCode= @pincode, CountryId = @countryId, @contactTypeId = @contactTypeId,
			Organization = @organization, Designation = @designation where Id = @contactId;

			set @processedId = @contactId;
			set @_status = 1;
			set @msg = 'Contact Detail Saved';

		end

	end try
	begin catch
		set @processedId = 0;
		set @msg = ERROR_MESSAGE();
		set @_status = 0;
	end catch
	select @processedId as ProcessedId, @msg as Message, @_status as Status;
end;
GO
/****** Object:  StoredProcedure [dbo].[CreateEmailSchedule]    Script Date: 4/15/2020 4:17:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[CreateEmailSchedule]
	@rundate datetime = NULL

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
if(@rundate is null)
begin
	SET @rundate = convert(datetime, convert(date,getdate()))
end
else
begin
	SET @rundate = convert(datetime, convert(date,@rundate))
end

declare @schedule table(SendDate datetime, RecordType varchar(20), RecordId int, TemplateName varchar(100), Notes varchar(500))

declare @locid int = 0, @intdate datetime = null, @curdate datetime = null, @class varchar(300) = '', @signingdate datetime = null, @contractcount int = 0
declare @contid int = 0, @catmonth int = 0

select LOC.Id, 
LastDate, NextDate,
Classification,
PrincipalDueDate,
CF.ConfirmDate,
TerminalDate, SigningDate,
CON.ContractCount into #LOCTEMP from TBL_LOC LOC 
left Join Finacle_InterestDue ID on LOC.LocAccountNo = ID.AccountId
left join (select AccountId, MIN(DueDate) PrincipalDueDate from Finacle_PrincipalDue where isnull(DueDate,'1920-01-01') > @rundate group by AccountId) PD on LOC.LocAccountNo = PD.AccountId 
left join (select LocId, MAX([Date]) ConfirmDate from TBL_LocBalance where [Date] is not null and YEAR([Date]) = YEAR(@rundate) and month([Date]) = 3 and day([Date]) = 31 group by LocId) CF on LOC.Id = CF.LocId
left join (
		select distinct LP.LocId, count(C.Id) ContractCount from TBL_LOC_Project LP inner join TBL_Projects P on LP.ProjectId = P.Id 
		inner join TBL_Projects_PQ PQ on P.Id = PQ.ProjectId inner join TBL_Contracts C on C.PQId = PQ.Id
		group by LP.LocId
		) CON on CON.LocId = LOC.Id
		
select CON.Id ContId,
TerminalDateOfDisbursement, EstimateValue, CT.Disbursement,
AdvPmtGrntExpiry,
PerBankGrntExpiry,
ScheduledCompDate,
ContractApprovalDate,
LOC.* into #CONTEMP
 from TBL_Contracts CON 
left join (select C.Id, MAX(LP.LocId) LocId from TBL_Contracts  C inner join TBL_Projects_PQ PQ on C.PQId = PQ.Id inner join TBL_Projects P on P.Id = PQ.ProjectId inner join TBL_LOC_Project LP on P.Id = LP.ProjectId group by C.Id ) LC on CON.Id = LC.Id 
left join (select AccountId, Max(CummulativeCredit) Disbursement from Finacle_Contract_Transanctions group by AccountId) CT on isnull(CON.CGSId,convert(varchar,CON.Id)) = CT.AccountId
left join #LOCTEMP LOC on LC.LocId = LOC.Id

--select * from #CONTEMP


if(day(@rundate)=1)
begin
	insert into @schedule (SendDate, RecordType, RecordId, TemplateName)
	select @rundate, 'LOC', Id, 'LocRules.BalanceConfirmation' from #LOCTEMP where ConfirmDate is null or YEAR(ConfirmDate) < (select case when MONTH(@rundate) < 4 then YEAR(@rundate)-1 else YEAR(@rundate) end)
end


while exists (select * from #LOCTEMP)
begin
	set @locid = 0
	set @locid = (select top 1 Id from #LOCTEMP)
	set @class = (select Classification from #LOCTEMP where Id = @locid)

	-----------------------------------------------------------------------------------IntDemand
	set @intdate = (select LastDate from #LOCTEMP where Id = @locid)
	if(@intdate is not null)
	begin
		if(MONTH(@intdate) = MONTH(@rundate) or MONTH(@intdate) = MONTH(@rundate)+1)
		begin
			set @curdate = DATEADD(day,1-day(@rundate),@rundate)
			while @curdate < dateadd(year, year(@rundate)-year(@intdate), @intdate)
			begin				
				if @curdate = @rundate
				begin
					
					insert into @schedule (SendDate, RecordType, RecordId, TemplateName, Notes)
					select @rundate, 'LOC', @locid, 'LocRules.InterestDemand', Convert(varchar(6), @intdate,107)
					--if(@class = 'Cat I as per 2015 Guidelines' && )
					--begin
						insert into @schedule (SendDate, RecordType, RecordId, TemplateName, Notes)
						select @rundate, 'LOC', @locid, 'LocRules.CommitmentFee' , Convert(varchar(6), @intdate,107)
					--end
				end
				set @curdate = dateadd(DAY,7,@curdate)
			end
		end
		else if(MONTH(@rundate) = 12 and MONTH(@intdate) = 1)
		begin
			set @curdate = DATEADD(day,1-day(@rundate),@rundate)
			while @curdate < dateadd(year, year(@rundate)-year(@intdate)+1, @intdate)
			begin				
				if @curdate = @rundate
				begin					
					insert into @schedule (SendDate, RecordType, RecordId, TemplateName, Notes)
					select @rundate, 'LOC', @locid, 'LocRules.InterestDemand' , Convert(varchar(6), @intdate,107)
					--if(@class = 'Cat I as per 2015 Guidelines' && )
					--begin
						insert into @schedule (SendDate, RecordType, RecordId, TemplateName, Notes)
						select @rundate, 'LOC', @locid, 'LocRules.CommitmentFee' , Convert(varchar(6), @intdate,107)
					--end
				end
				set @curdate = dateadd(DAY,7,@curdate)
			end
		end
	end
	set @intdate = NULL
	set @intdate = (select NextDate from #LOCTEMP where Id = @locid)
	if(@intdate is not null)
	begin
		if(MONTH(@intdate) = MONTH(@rundate) or MONTH(@intdate) = MONTH(@rundate)+1)
		begin
			set @curdate = DATEADD(day,1-day(@rundate),@rundate)
			while @curdate < dateadd(year, year(@rundate)-year(@intdate), @intdate)
			begin				
				if @curdate = @rundate
				begin
					
					insert into @schedule (SendDate, RecordType, RecordId, TemplateName, Notes)
					select @rundate, 'LOC', @locid, 'LocRules.InterestDemand', Convert(varchar(6), @intdate,107)
					--if(@class = 'Cat I as per 2015 Guidelines' && )
					--begin
						insert into @schedule (SendDate, RecordType, RecordId, TemplateName, Notes)
						select @rundate, 'LOC', @locid, 'LocRules.CommitmentFee' , Convert(varchar(6), @intdate,107)
					--end
				end
				set @curdate = dateadd(DAY,7,@curdate)
			end
		end
		else if(MONTH(@rundate) = 12 and MONTH(@intdate) = 1)
		begin
			set @curdate = DATEADD(day,1-day(@rundate),@rundate)
			while @curdate < dateadd(year, year(@rundate)-year(@intdate)+1, @intdate)
			begin				
				if @curdate = @rundate
				begin					
					insert into @schedule (SendDate, RecordType, RecordId, TemplateName, Notes)
					select @rundate, 'LOC', @locid, 'LocRules.InterestDemand' , Convert(varchar(6), @intdate,107)
					--if(@class = 'Cat I as per 2015 Guidelines' && )
					--begin
						insert into @schedule (SendDate, RecordType, RecordId, TemplateName, Notes)
						select @rundate, 'LOC', @locid, 'LocRules.CommitmentFee' , Convert(varchar(6), @intdate,107)
					--end
				end
				set @curdate = dateadd(DAY,7,@curdate)
			end
		end
	end
	-----------------------------------------------------------------------------------IntDemand

	-----------------------------------------------------------------------------------PriDemand
	set @intdate = NULL
	set @intdate = (select PrincipalDueDate from #LOCTEMP where Id = @locid)
	if(@intdate is not null)
	begin
		if(((MONTH(@intdate) = MONTH(@rundate) or MONTH(@intdate) = MONTH(@rundate)+1) and YEAR(@intdate) = YEAR(@rundate)) or (MONTH(@rundate) = 12 and MONTH(@rundate) = 1 and YEAR(@intdate) = YEAR(@rundate)+1))
		begin
			set @curdate = DATEADD(day,1-day(@rundate),@rundate)
			while @curdate < @intdate
			begin				
				if @curdate = @rundate
				begin					
					insert into @schedule (SendDate, RecordType, RecordId, TemplateName, Notes)
					select @rundate, 'LOC', @locid, 'LocRules.PrincipalDemand' , Convert(varchar(6), @intdate,107)
				end
				set @curdate = dateadd(DAY,7,@curdate)
			end
		end
	end
	-----------------------------------------------------------------------------------PriDemand

	-----------------------------------------------------------------------------------TerminalSigning
	set @intdate = NULL
	set @intdate = (select TerminalDate from #LOCTEMP where Id = @locid)
	set @signingdate = NULL
	set @signingdate = (select SigningDate from #LOCTEMP where Id = @locid)

	if(@intdate is not null and @signingdate is null)
	begin
			set @curdate = NULL
			set @curdate = DATEADD(month,-6,@intdate)
			while @curdate <= @rundate
			begin				
				if @curdate = @rundate
				begin					
					insert into @schedule (SendDate, RecordType, RecordId, TemplateName, Notes)
					select @rundate, 'LOC', @locid, 'LocRules.AgreementSigningExp' , Convert(varchar(6), @intdate,107)
				end
				set @curdate = dateadd(DAY,15,@curdate)
			end

	end
	-----------------------------------------------------------------------------------TerminalSigning

	-----------------------------------------------------------------------------------TerminalContract
	set @contractcount = NULL
	set @contractcount = (select ContractCount from #LOCTEMP where Id = @locid)
	set @signingdate = NULL
	set @signingdate = (select SigningDate from #LOCTEMP where Id = @locid)

	if(isnull(@contractcount,0) = 0 and @signingdate is not null)
	begin
			set @curdate = NULL
			set @curdate = DATEADD(month,18-6,@signingdate)
			while @curdate <= @rundate
			begin				
				if @curdate = @rundate
				begin					
					insert into @schedule (SendDate, RecordType, RecordId, TemplateName, Notes)
					select @rundate, 'LOC', @locid, 'LocRules.ContractsExpiry' , Convert(varchar(6), @signingdate,107)
				end
				set @curdate = dateadd(month,1,@curdate)
			end

	end
	-----------------------------------------------------------------------------------TerminalContract


	delete from #LOCTEMP where Id = @locid
end

drop table #LOCTEMP


insert into @schedule (SendDate, RecordType, RecordId, TemplateName, Notes)
select @rundate, 'Project', PQ.Id, 'ProjectRules.BidsSubmissionPQ',  Convert(varchar(6), ApplicationEnd,107) from TBL_Projects P inner join TBL_Projects_PQ PQ on P.Id = PQ.ProjectId 
where isnull(ApplicationEnd,'1900-01-01') >= @rundate and datediff(day, @rundate, ApplicationEnd) <= 7 


while exists (select * from #CONTEMP)
begin
	set @contid = 0
	set @contid = (select top 1 ContId from #CONTEMP)
	set @class = (select Classification from #CONTEMP where ContId = @contid)

	-----------------------------------------------------------------------------------TerminalDisb
	set @signingdate = NULL
	set @signingdate = (select TerminalDateOfDisbursement from #CONTEMP where ContId = @contid)

	if exists(select * from #CONTEMP where ContId = @contid and isnull(Disbursement,0) < EstimateValue and TerminalDateOfDisbursement is not null)
	begin
			set @curdate = NULL
			set @curdate = DATEADD(month,-6,@signingdate)
			while @curdate <= @rundate
			begin				
				if @curdate = @rundate
				begin					
					insert into @schedule (SendDate, RecordType, RecordId, TemplateName, Notes)
					select @rundate, 'Contract', @contid, 'ContractRules.TerminalDateofDisbursement',  Convert(varchar(6), @signingdate,107) 
				end
				set @curdate = dateadd(month,1,@curdate)
			end

	end
	-----------------------------------------------------------------------------------TerminalDisb

	-----------------------------------------------------------------------------------AdvancePaymentGuarantee
	set @signingdate = NULL
	set @signingdate = (select AdvPmtGrntExpiry from #CONTEMP where ContId = @contid)

	if (@signingdate is not null)
	begin
			set @curdate = NULL
			set @curdate = DATEADD(month,-6,@signingdate)
			while @curdate <= @rundate
			begin				
				if @curdate = @rundate
				begin					
					insert into @schedule (SendDate, RecordType, RecordId, TemplateName, Notes)
					select @rundate, 'Contract', @contid, 'ContractRules.AdvancePaymentGuarantee',  Convert(varchar(6), @signingdate,107) 
				end
				set @curdate = dateadd(month,1,@curdate)
			end

	end
	-----------------------------------------------------------------------------------AdvancePaymentGuarantee

	-----------------------------------------------------------------------------------PerformanceGuarantee
	set @signingdate = NULL
	set @signingdate = (select PerBankGrntExpiry from #CONTEMP where ContId = @contid)

	if (@signingdate is not null)
	begin
			set @curdate = NULL
			set @curdate = DATEADD(month,-6,@signingdate)
			while @curdate <= @rundate
			begin				
				if @curdate = @rundate
				begin					
					insert into @schedule (SendDate, RecordType, RecordId, TemplateName, Notes)
					select @rundate, 'Contract', @contid, 'ContractRules.PerformanceGuarantee',  Convert(varchar(6), @signingdate,107) 
				end
				set @curdate = dateadd(month,1,@curdate)
			end

	end
	-----------------------------------------------------------------------------------PerformanceGuarantee

	-----------------------------------------------------------------------------------ScheduleCompleteDate
	set @signingdate = NULL
	set @signingdate = (select ScheduledCompDate from #CONTEMP where ContId = @contid)

	if (@signingdate is not null)
	begin
			set @curdate = NULL
			set @curdate = DATEADD(month,-6,@signingdate)
			while @curdate <= @rundate
			begin				
				if @curdate = @rundate
				begin					
					insert into @schedule (SendDate, RecordType, RecordId, TemplateName, Notes)
					select @rundate, 'Contract', @contid, 'ContractRules.ScheduleCompleteDate',  Convert(varchar(6), @signingdate,107) 
				end
				set @curdate = dateadd(month,1,@curdate)
			end

	end
	-----------------------------------------------------------------------------------ScheduleCompleteDate

	-----------------------------------------------------------------------------------CommitmentFee
	set @signingdate = NULL
	set @signingdate = (select ContractApprovalDate from #CONTEMP where ContId = @contid)
	
	if(@class = 'Cat I as per 2015 Guidelines')
	begin
		set @catmonth = 12
	end
	else if (@class = 'Cat II as per 2015 Guidelines')
	begin
		set @catmonth = 2
	end
	else if (@class = 'Cat III as per 2015 Guidelines')
	begin
		set @catmonth = 2
	end

	set @intdate = NULL
	set @intdate = (select LastDate from #CONTEMP where ContId = @contid)
	if (@signingdate is not null)
	begin
			set @curdate = NULL
			set @curdate = DATEADD(month,@catmonth,@signingdate)
			if(@intdate is not null)
			begin
				set @intdate = (select case when month(@curdate) <= month(@intdate) then DATEADD(YEAR,DATEDIFF(year,@intdate,@curdate),@intdate) else DATEADD(YEAR,DATEDIFF(year, @intdate,@curdate)+1,@intdate) end)
			end
			else
			begin
				set @intdate = @rundate
			end

			while @curdate <= @rundate and @curdate <= @intdate
			begin				
				if @curdate = @rundate
				begin					
					insert into @schedule (SendDate, RecordType, RecordId, TemplateName, Notes)
					select @rundate, 'Contract', @contid, 'ContractRules.FirstCommitmentFeeCat1',  Convert(varchar, DATEADD(month,@catmonth,@signingdate),107) 
				end
				set @curdate = dateadd(month,1,@curdate)
			end

	end

	set @intdate = NULL
	set @intdate = (select NextDate from #CONTEMP where ContId = @contid)
	if (@signingdate is not null)
	begin
			set @curdate = NULL
			set @curdate = DATEADD(month,@catmonth,@signingdate)
			if(@intdate is not null)
			begin
				set @intdate = (select case when month(@curdate) <= month(@intdate) then DATEADD(YEAR,DATEDIFF(year,@intdate,@curdate),@intdate) else DATEADD(YEAR,DATEDIFF(year, @intdate,@curdate)+1,@intdate) end)
			end
			else
			begin
				set @intdate = @rundate
			end

			while @curdate <= @rundate and @curdate <= @intdate
			begin				
				if @curdate = @rundate
				begin					
					insert into @schedule (SendDate, RecordType, RecordId, TemplateName, Notes)
					select @rundate, 'Contract', @contid, 'ContractRules.FirstCommitmentFeeOCat',  Convert(varchar, DATEADD(month,@catmonth,@signingdate),107) 
				end
				set @curdate = dateadd(month,1,@curdate)
			end

	end
	-----------------------------------------------------------------------------------CommitmentFee



	delete from #CONTEMP where ContId = @contid
end

drop table #CONTEMP

--select * from [TBL_EmailSchedule]

delete from [TBL_EmailSchedule] where SendDate = @rundate

insert into [TBL_EmailSchedule](RecordType, RecordId, SendDate, TemplateName, Notes, IsActive)
select distinct RecordType, RecordId, SendDate, TemplateName, Notes, 1 from @schedule S
where not exists (select * from [TBL_EmailSchedule] where RecordId = S.RecordId and RecordType = S.RecordType and SendDate = S.SendDate and TemplateName = S.TemplateName)


END
GO
/****** Object:  StoredProcedure [dbo].[CreateUpdate_Loc]    Script Date: 4/15/2020 4:17:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[CreateUpdate_Loc] 
@signingDate         DATETIME, 
@terminalDate        DATETIME, 
@meaDate             DATETIME, 
@mdDate              DATETIME, 
@offerDate           DATETIME, 
@omNumber            VARCHAR(30), 
@amountAllocated     FLOAT, 
@interest            FLOAT, 
@commitmentFee       FLOAT, 
@managementFee       FLOAT, 
@equalization        VARCHAR(max), 
@mea_percentage      FLOAT, 
@dea_percentage      FLOAT, 
@approvalType        VARCHAR(max), 
@tenure              INT, 
@moratorium          INT, 
@indianContribution  FLOAT, 
@purpose             VARCHAR(max), 
@locId               INT, 
@countryId           INT, 
@approvalBy          INT, 
@approvalDate        DATETIME, 
@ammendmentNumber    INT, 
@goiDeedDate         DATETIME, 
@effectiveDate       DATETIME, 
@aggrementAmmendDate DATETIME, 
@name                VARCHAR(300), 
@locNumber           VARCHAR(100), 
@locAccountNumber    VARCHAR(30), 
@mea_type            VARCHAR(max), 
@dea_type            VARCHAR(max), 
@classification      VARCHAR(50), 
@deadate             DATETIME, 
@specialcondition    VARCHAR(max), 
@user                VARCHAR(200),
@vnote               VARCHAR(max),
@anote				 VARCHAr(max),
@interesttype		 varchar(max)

AS 
  BEGIN 
      SET nocount ON; 

      DECLARE @msg VARCHAR(max); 
      DECLARE @status BIT; 
      DECLARE @processedId INT; 

      begin try 
	  	  
		declare @isamend int = 
		(select count(*) from tbl_loc where id = @locId and agreementamendmentdate = @aggrementAmmendDate and omnumber = @omNumber and terminaldate = @terminalDate 
		and indiancontribution = @indianContribution and mdappdate = @mdDate and DeaDate=@deadate and meaappdate = @meaDate and offerletterdate = @offerDate and interestrate = @interest 
		and commitmentfee = @commitmentFee and managementfee= @managementFee and signingdate = @signingDate and mea_type = @mea_type and 
		dea_type = @dea_type and  mea_percentage = @mea_percentage and dea_percentage = @dea_percentage and tenure = @tenure 
		and moratorium = @moratorium and InterestType = @interesttype and Purpose = @purpose and specialcondition = @specialcondition and TotalAmount = @amountAllocated
		)


      IF @locId = 0 
        BEGIN 
            INSERT INTO tbl_loc (signingdate, terminaldate, meaappdate, mdappdate, offerletterdate, omnumber, totalamount, interestrate, commitmentfee, managementfee, 
                         interestequalization, mea_percentage, dea_percentage, approvaltype, tenure, moratorium, indiancontribution, purpose, countryid, approvalby, approvaldate, amendmentnumber, 
                         goideeddate, effectivedate, agreementamendmentdate, [name], locaccountno, locnumber, mea_type, dea_type, classification, deadate, VerificationNote, SpecialCondition, InterestType) 
            VALUES     (@signingDate, @terminalDate, @meaDate, @mdDate, @offerDate, UPPER(@omNumber), @amountAllocated, @interest, @commitmentFee, @managementFee, 
                        @equalization, @mea_percentage, @dea_percentage, @approvalType, @tenure, @moratorium, @indianContribution, @purpose, @countryId, @approvalBy, @approvalDate, @ammendmentNumber, 
                        @goiDeedDate, @effectiveDate, @aggrementAmmendDate, @name, UPPER(@locAccountNumber), UPPER(@locNumber), @mea_type, @dea_type, @classification, @deadate,@vnote, @specialcondition, @interesttype); 

            SET @locId = Scope_identity(); 
        END 
      ELSE 
        BEGIN 
            UPDATE tbl_loc 
            SET    signingdate = @signingDate, 
                   terminaldate = @terminalDate, 
                   meaappdate = @meaDate, 
                   mdappdate = @mdDate, 
                   offerletterdate = @offerDate, 
                   interestrate = @interest, 
                   commitmentfee = @commitmentFee, 
                   managementfee = @managementFee, 
                   interestequalization = @equalization, 
                   mea_percentage = @mea_percentage, 
				   dea_percentage = @dea_percentage, 
                   approvaltype = @approvalType, 
                   tenure = @tenure, 
                   moratorium = @moratorium, 
                   indiancontribution = @indianContribution, 
                   purpose = @purpose, 
                   omnumber = UPPER(@omNumber), 
                   totalamount = @amountAllocated, 
                   countryid = @countryId, 
                   [name] = @name, 
                   locaccountno = UPPER(@locAccountNumber), 
                   locnumber = UPPER(@locNumber), 
                   agreementamendmentdate = @aggrementAmmendDate, 
                   deadate = @deadate, 
                   mea_type = @mea_type, 
				   dea_type = @dea_type, 
                   classification = @classification,
				   VerificationNote = @vnote,
				   SpecialCondition = @specialcondition,
				   InterestType = @interesttype,
				   EffectiveDate=@effectiveDate
            WHERE  id = @locId; 

            
        END 


		if @isamend = 0
		begin
		INSERT INTO tbl_loc_amendments (loc_id, auditdate, amendedby, agreementamendmentdate, omnumber, terminaldate, indiancontribution, mdappdate, deaappdate, meaappdate, InterestType,
            offerletterdate, interestrate, commitmentfee, managementfee, signingdate, interestequalization, mea_type, dea_type, mea_percentage, dea_percentage, 
			tenure, moratorium, specialcondition, LOCPurpose, AmendmentNote, AmountAllocated) 
		SELECT @locId, Getdate(), @user, @aggrementAmmendDate, UPPER(@omNumber), @terminalDate, @indianContribution, @mdDate, @deadate, @meaDate, @interesttype,
				@offerDate, @interest, @commitmentFee, @managementFee, @signingDate, @equalization, @mea_type, @dea_type, round(@mea_percentage,2), round(@dea_percentage,2), 
				@tenure, @moratorium, @specialcondition, @purpose, @anote, @amountAllocated
		end


	  SET @processedId = @locId; 
      SET @status = 1; 
      SET @msg = ''; 

      end try 
      begin catch 
             /*SELECT ERROR_NUMBER() AS ErrorNumber 
             ,ERROR_SEVERITY() AS ErrorSeverity 
             ,ERROR_STATE() AS ErrorState 
             ,ERROR_PROCEDURE() AS ErrorProcedure 
             ,ERROR_LINE() AS ErrorLine 
             ,ERROR_MESSAGE() AS ErrorMessage;*/ 
             set @processedId = 0; 
             set @msg = ERROR_MESSAGE(); 
             set @status = 0; 
      end catch; 
      SELECT @processedId AS ProcessedId, 
             @msg         AS Message, 
             @status      AS Status 
  END;
GO
/****** Object:  StoredProcedure [dbo].[Get_Color_ChangesFor_Loc]    Script Date: 4/15/2020 4:17:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Procedure [dbo].[Get_Color_ChangesFor_Loc]
@locId int
as
begin
	declare @colorVal nvarchar(10);
	set @colorVal = 'Yellow';
	Select a.Id, a.Loc_ID, 
	a.AuditDate, a.AmendedBy,  a.AgreementAmendmentDate, a.TerminalDate, a.IndianContribution,
	a.MdAppDate, a.DEA_Type, a.MEAAppDate, a.DEAAppDate, a.OfferLetterDate, a.InterestRate, a.CommitmentFee, a.ManagementFee,
	a.SigningDate, a.InterestEqualization, a.Type, a.Percentage, a.Tenure, a.Moratorium, a.SpecialCondition, a.OmNumber, 
	a.LOCPurpose, a.InterestType, a.AmendmentNote, a.AmountAllocated, a.MEA_Type, a.MEA_Percentage, a.DEA_Percentage,
	Min(b.Id) CompID 
	into #grouped_table
	from TBL_LOC_Amendments as a
	left join TBL_LOC_Amendments AS b 
	on a.LOC_Id = b.LOC_Id and b.Id > a.Id 
	where a.LOC_Id = @locId 
	Group By a.Loc_ID, a.Id, a.AuditDate, a.AmendedBy,  a.AgreementAmendmentDate, a.TerminalDate, a.IndianContribution,
	a.MdAppDate, a.DEA_Type, a.MEAAppDate, a.DEAAppDate, a.OfferLetterDate, a.InterestRate, a.CommitmentFee, a.ManagementFee,
	a.SigningDate, a.InterestEqualization, a.Type, a.Percentage, a.Tenure, a.Moratorium, a.SpecialCondition, a.OmNumber, 
	a.LOCPurpose, a.InterestType, a.AmendmentNote, a.AmountAllocated, a.MEA_Type, a.MEA_Percentage, a.DEA_Percentage
	order by a.Id
	
	Select a.Id, a.Loc_ID, a.CompID,  
	Case when a.AuditDate <> b.AuditDate then @colorVal else '' end AuditDate, 
	Case when a.AmendedBy <> b.AmendedBy then @colorVal else '' end AmendedBy,  
	Case when a.AgreementAmendmentDate <> b.AgreementAmendmentDate then @colorVal else '' end AgreementAmendmentDate, 
	Case when a.TerminalDate <> b.TerminalDate then @colorVal else '' end TerminalDate, 
	Case when a.IndianContribution <> b.IndianContribution then @colorVal else '' end IndianContribution,
	Case when a.MdAppDate <> b.MdAppDate then @colorVal else '' end MdAppDate, 
	Case when a.DEA_Type <> b.DEA_Type then @colorVal else '' end DEA_Type, 
	Case when a.MEAAppDate <> b.MEAAppDate then @colorVal else '' end MEAAppDate, 
	Case when a.DEAAppDate <> b.DEAAppDate then @colorVal else '' end DEAAppDate, 
	Case when a.OfferLetterDate <> b.OfferLetterDate then @colorVal else '' end OfferLetterDate, 
	Case when a.InterestRate <> b.InterestRate then @colorVal else '' end InterestRate, 
	Case when a.CommitmentFee <> b.CommitmentFee then @colorVal else '' end CommitmentFee, 
	Case when a.ManagementFee <> b.ManagementFee then @colorVal else '' end ManagementFee,
	Case when a.SigningDate <> b.SigningDate then @colorVal else '' end SigningDate, 
	Case when a.InterestEqualization <> b.InterestEqualization then @colorVal else '' end InterestEqualization, 
	Case when a.Type <> b.Type then @colorVal else '' end Type, 
	Case when a.Percentage <> b.Percentage then @colorVal else '' end Percentage,
	Case when a.Tenure <> b.Tenure then @colorVal else '' end Tenure, 
	Case when a.Moratorium <> b.Moratorium then @colorVal else '' end Moratorium, 
	Case when a.SpecialCondition <> b.SpecialCondition then @colorVal else '' end SpecialCondition, 
	Case when a.OmNumber <> b.OmNumber then @colorVal else '' end OmNumber, 
 	Case when a.LOCPurpose <> b.LOCPurpose then @colorVal else '' end LOCPurpose, 
	Case when a.InterestType <> b.InterestType then @colorVal else '' end InterestType, 
	Case when a.AmendmentNote <> b.AmendmentNote then @colorVal else '' end AmendmentNote, 
	Case when a.AmountAllocated <> b.AmountAllocated then @colorVal else '' end AmountAllocated, 
	Case when a.MEA_Type <> b.MEA_Type then @colorVal else '' end MEA_Type, 
	Case when a.MEA_Percentage <> b.MEA_Percentage then @colorVal else '' end MEA_Percentage, 
	Case when a.DEA_Percentage <> b.DEA_Percentage then @colorVal else '' end DEA_Percentage
	from #grouped_table a 
	inner join TBL_LOC_Amendments b
	on b.Id = a.CompID
	where CompID is not null

	
end

--Exec Get_Color_ChangesFor_Loc 11
--Select * from TBL_LOC_Amendments
GO
/****** Object:  StoredProcedure [dbo].[Get_Contract_Types]    Script Date: 4/15/2020 4:17:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create Procedure [dbo].[Get_Contract_Types]
as
Begin
 Select  Name FROM TBL_Contract_Types;
end
GO
/****** Object:  StoredProcedure [dbo].[Get_Country_Alphabets]    Script Date: 4/15/2020 4:17:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create Procedure [dbo].[Get_Country_Alphabets] as
begin
Select * from( Select Distinct SUBSTRING(Name, 1, 1) Letter  from TBL_Country) Alphabets Order By Letter
end;



GO
/****** Object:  StoredProcedure [dbo].[GetLocs]    Script Date: 4/15/2020 4:17:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetLocs] 
	@Id int = 0
AS
BEGIN
	select * from TBL_LOC
END
GO
/****** Object:  StoredProcedure [dbo].[GetProjectContracts]    Script Date: 4/15/2020 4:17:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE pROCEDURE [dbo].[GetProjectContracts]
@projectId int,
@pqId int
as
begin
	Select a.Id as ProjectPqId, a.PQRefNumber PqNo, b.PackageId, b.PackageName, b.PackageDisplayId, b.EstimateValue, b.TypeOfPackage,
	b.Id as ContractId , b.ContractorName
	from TBL_Projects_PQ as a inner join TBL_Contracts as b
	on a.Id = b.PQId where 
	(IsNull(@projectId, 0)= 0 or a.ProjectId = @projectId )
	and (IsNull(@pqId, 0) = 0 or a.Id = @pqId);
end;

GO
/****** Object:  StoredProcedure [dbo].[GetProjectCountries]    Script Date: 4/15/2020 4:17:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Procedure [dbo].[GetProjectCountries]
@projectId int
as
begin
	declare @countries varchar(max);
	set @countries =  stuff((
								select ',' + c.Name
								from (Select c.Name  from TBL_LOC_Project lp inner join TBL_LOC loc ON lp.LocId = loc.Id 
									left join TBL_Country c on loc.CountryId = c.Id
									where ProjectId = @projectId) c
								where c.Name = Name
								for xml path('')
							),1,1,'');
	Select @countries as Countries;
end;
GO
/****** Object:  StoredProcedure [dbo].[GetProjectList]    Script Date: 4/15/2020 4:17:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Procedure [dbo].[GetProjectList]
@name varchar(20)
as
begin
	declare @cur_Projects cursor;
	declare @projectId int;
	declare @countries varchar(max);
	declare @tmp_projects table
	(Id int, Code varchar(100), Name varchar(max), Countries varchar(max), 
	TeamMembers varchar(max), ProjectValue decimal(18,2), ProjectProgress float default(0), 
	FinancialProgress float default(0),
	ProjectStatus varchar(100), CrOn varchar(50));

	Insert into @tmp_projects (Id, Code, Name, ProjectValue, ProjectProgress, FinancialProgress, ProjectStatus,
	CrOn) (Select a.Id, a.Code, a.Name, a.ProjectValue, a.Progress, a.FinancialProgress, b.Name,
	Convert(varchar, a.CrOn, 105)  from TBL_Projects a left join TBL_Status as b on b.Id = a.Status) ;

	set @cur_Projects = cursor Fast_Forward For 
	Select Id from @tmp_projects;

	open @cur_Projects;

	Fetch Next from @cur_Projects into @projectId;
	while @@FETCH_STATUS = 0
	begin
		set @countries =
						 stuff((
								select ',' + c.Name
								from (Select c.Name  from TBL_LOC_Project lp inner join TBL_LOC loc ON lp.LocId = loc.Id 
									left join TBL_Country c on loc.CountryId = c.Id
									where ProjectId = @projectId) c
								where c.Name = Name
								for xml path('')
							),1,1,'');
		
		update @tmp_projects set Countries = @countries where Id = @projectId;

		Fetch Next from @cur_Projects into @projectId;
	end
	close @cur_Projects;
	deallocate @cur_Projects;


	if @name is not null and @name<> ''
	begin
		select Id, Code, Name as ProjectName, Countries, TeamMembers, ProjectValue, ISNULL(ProjectProgress, 0) ProjectProgress,
		ISNULL(FinancialProgress, 0) FinancialProgress, ISNULL(ProjectStatus, '') ProjectStatus, CrOn as CreatedOn from  @tmp_projects
		where Name like '%'+@name + '%';
	end
	else
	begin
		select Id, Code, Name as ProjectName, Countries, TeamMembers, ProjectValue, ISNULL(ProjectProgress, 0) ProjectProgress,
		ISNULL(FinancialProgress, 0) FinancialProgress, ISNULL(ProjectStatus, '') ProjectStatus, CrOn as CreatedOn from  @tmp_projects
	end


end;
GO
/****** Object:  StoredProcedure [dbo].[Insert_Monthly_Status]    Script Date: 4/15/2020 4:17:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


                    
                    CREATE PROCEDURE [dbo].[Insert_Monthly_Status] --'2020-04-01'
                     @month datetime
                    AS
                    BEGIN 

					declare @datadate datetime = convert(datetime, convert(varchar(4), year(@month)) +'-'+ convert(varchar(2), month(@month)) +'-01')
					set @datadate = DATEADD(month,-1,@datadate)

					delete from MonthlyFinacleData where datamonth = @datadate

					INSERT INTO MonthlyFinacleData ([DataMonth],LOC_Id,LOC_LocNumber,LOC_SanctionedAmount,LOC_DisbursedAmount,LOC_FirstDisbursement,
											LOC_ProjectCount,LOC_AmountRepaid,LOC_AmountOutstanding,LOC_Principaloverdue,LOC_Interestoverdue,LOC_PrincipalRepaymentStart,
											LOC_PrincipalRepaymentEnd,LOC_InterestDueDates,LOC_PrincipalDueDates,Project_Id,Project_Code,Contract_Id,
											Contract_SanctionedAmount,Contract_DisbursedAmount)

                        select
 
 [DataMonth]=@datadate,
LOC.Id [LOC_Id],

LOC.LocNumber [LOC_LocNumber],

 

isnull((select MAX(SanctionLimit) from Finacle_Disbursement where FORACID = LOC.LocAccountNo),0) [LOC_Sanctioned Amount]

         ,isnull((select SUM(DisbAmount) from Finacle_Disbursement where FORACID = LOC.LocAccountNo),0) [LOC_Disbursed Amount]

         ,isnull((select MIN(DisDate) from Finacle_Disbursement where FORACID = LOC.LocAccountNo),0) [LOC_First Disbursement]

         ,(select count(Id) from TBL_LOC_Project where LocId = LOC.Id) [LOC_ProjectCount]

         ,isnull((select SUM(PrincipalCollection) from  Finacle_LocFinancials where FORACID = LOC.LocAccountNo),0) [LOC_Amount Repaid]

         ,isnull((select SUM(LoanOutstanding) from  Finacle_LocFinancials where FORACID = LOC.LocAccountNo),0) [LOC_Amount Outstanding]

         ,isnull((select SUM(PrincipalOverdue) from  Finacle_LocFinancials where FORACID = LOC.LocAccountNo),0) [LOC_Principal Overdue]

         ,isnull((select SUM(InterestOverdue) from  Finacle_LocFinancials where FORACID = LOC.LocAccountNo),0) [LOC_Interest Overdue]

         ,isnull((select min(FlowStart) from Finacle_RepaymentSchedule where FlowId = 'PRDEM' and FORACID = LOC.LocAccountNo),'')[LOC_Principal Repayment Start]

         ,isnull((select max(FlowStart) from Finacle_RepaymentSchedule where FlowId = 'PRDEM' and FORACID = LOC.LocAccountNo),'')[LOC_Principal Repayment End]

         ,(select convert(varchar(6),NextDate, 113) + ', '+ convert(varchar(6),LastDate, 113) from Finacle_InterestDue where AccountId = LOC.LocAccountNo) [LOC_Interest Due Dates]

         ,isnull((

                      SELECT

                      --[AccountId],

                      STUFF((

                      SELECT top 2 ', ' + convert(varchar(6),DueDate, 113)

                           FROM Finacle_PrincipalDue

                           WHERE DemandType = 'PRDEM' and AccountId = LOC.LocAccountNo and DueDate > getdate() order by DueDate

                           FOR XML PATH(''),TYPE).value('(./text())[1]','VARCHAR(MAX)')

                      ,1,2,'') AS NameValues

                    FROM Finacle_PrincipalDue Results

                    where AccountId = LOC.LocAccountNo

                    GROUP BY AccountId),'') [LOC_Principal Due Dates]

 

                    ,

 

P.Id [Project_Id],

P.Code [Project_Code],

CON.Id [Contract_Id],

(select MAX(SanctionedAmount) from Finacle_Contract_Transanctions where Particulars not like '%reversal%' and AccountId = CON.CGSId) [Contract_Sanctioned Amount]

,(select MAX(CummulativeDebit) from Finacle_Contract_Transanctions where Particulars not like '%reversal%' and AccountId = CON.CGSId) [Contract_Disbursed Amount]

 

from TBL_LOC LOC inner join TBL_LOC_Project LP on LOC.Id = LP.LocId

inner join TBL_Projects P on LP.ProjectId = P.Id

inner join TBL_Projects_PQ PQ on P.Id = PQ.ProjectId

inner join TBL_Contracts CON on CON.PQId = PQ.Id 
										
										  
                    END
					
            
GO
/****** Object:  StoredProcedure [dbo].[LinkProjectContacts]    Script Date: 4/15/2020 4:17:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create procedure [dbo].[LinkProjectContacts]
@projectId int,
@contactId int
as
begin
	declare @msg varchar(max);
	declare @_status bit;
	declare @processedId int;
	begin try
		insert into TBL_Project_Contacts(ProjectId, ContactId) 
		values(@projectId, @contactId);
		set @processedId = SCOPE_IDENTITY();
		set @msg = 'Contact Updated';
		set @_status = 1;

	end try
	begin catch
		set @processedId = 0;
		set @msg = ERROR_MESSAGE();
		set @_status = 0;
	end catch
	select @processedId as ProcessedId, @msg as Message, @_status as Status;
end;
GO

/****** Object:  StoredProcedure [dbo].[report_Completed_Projects]    Script Date: 4/15/2020 4:17:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

                    
                    Create PROCEDURE [dbo].[report_Completed_Projects]
                    
                    AS
                    BEGIN                    
                        select distinct  [LOC_Region] as [LOC_Region],[LOC_Country] as [LOC_Country],[Project_Name] as [Project_Name],[Project_ProjectValue] as [Project_ProjectValue],[Contract_ContractApprovalDate] as [Contract_ContractApprovalDate],[Contract_ScheduledCompDate] as [Contract_ScheduledCompDate],[Contract_ActualCompletionDate] as [Contract_ActualCompletionDate] from report_All where 1 = 1 
                    END

            
GO
/****** Object:  StoredProcedure [dbo].[report_Contract_Details]    Script Date: 4/15/2020 4:17:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

                    
                    Create PROCEDURE [dbo].[report_Contract_Details]
                    
                    AS
                    BEGIN                    
                        select distinct  [LOC_Region] as [LOC_Region],[LOC_Country] as [LOC_Country],[Project_Address] as [Project_Address],[Project_Name] as [Project_Name],[Contract_ContractorName] as [Contract_ContractorName],[Contract_ContractApprovalDate] as [Contract_ContractApprovalDate],[Project_ProjectValue] as [Project_ProjectValue],[Contract_EstimateValue] as [Contract_EstimateValue],[Project_Sector] as [Project_Sector],[Contract_ContractType] as [Contract_ContractType],[Contract_ScheduledCompDate] as [Contract_ScheduledCompDate],[Contract_RevisedCompletionDate] as [Contract_RevisedCompletionDate],[Contract_PACDate] as [Contract_PACDate],[Contract_FACDate] as [Contract_FACDate],[Project_Progress] as [Project_Progress],[Project_FinancialProgress] as [Project_FinancialProgress],[LOC_IndianContribution] as [LOC_IndianContribution],[Contract_Note] as [Contract_Note] from report_All where 1 = 1 
                    END

            
GO

/****** Object:  StoredProcedure [dbo].[report_GOI_Guarantees_Issued]    Script Date: 4/15/2020 4:17:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

                    
                    CREATE PROCEDURE [dbo].[report_GOI_Guarantees_Issued]
                    
                    AS
                    BEGIN                    
                        
						
						select distinct  [Name] as [Borrower],[LocNumber] as [LocNumber], LocAccountNo [Account Number], [AmountAllocated]/1000000 as [LOC Amount (USD Mn)],[Purpose] as [Purpose],
						[OM Date] as [Date of GOI Approval],[SigningDate] as [Date of Signing of LOC],[OmNumber] as [OM Number] 
						from report_LOC where 1 = 1 
                    
					
					
					END

            
GO
/****** Object:  StoredProcedure [dbo].[report_LOC_Commitments]    Script Date: 4/15/2020 4:17:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

                    
                    CREATE PROCEDURE [dbo].[report_LOC_Commitments]
                    
                    AS
                    BEGIN                    
                        select distinct LocNumber [LOC Number], LocAccountNo [Account Number], [Name] as [Borrower],[Country] as [Country],[OM Date] as [Guaranteed by GOI],[AmountAllocated]/1000000 as [LOC AMount (USD Mn)] from report_LOC where 1 = 1 
                    END

            
GO

/****** Object:  StoredProcedure [dbo].[report_LOC_Details_1]    Script Date: 4/15/2020 4:17:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

                    
                    Create PROCEDURE [dbo].[report_LOC_Details_1]
                    AS
                    BEGIN                    
                        select distinct (case when isdate([OM Date])=1 then
                                        (case when month([OM Date]) < 4 then convert(varchar(4), year([OM Date]) - 1) + '-' + convert(varchar(4), year([OM Date]))
                                            when month([OM Date]) >= 4 then convert(varchar(4), year([OM Date])) + '-' + convert(varchar(4), year([OM Date]) + 1)
                                        else '' end)
                                        else ''
                                        end) FinancialYear, [Region] as [Region],[Country] as [Country],[Name] as [Name],[MdAppDate] as [MdAppDate],[MEAAppDate] as [MEAAppDate],[AmountAllocated] as [AmountAllocated],[Purpose] as [Purpose],[SigningDate] as [SigningDate],[Disbursed Amount] as [Disbursed Amount],[Amount Repaid] as [Amount Repaid],[LOC Status] as [LOC Status],[OM Date] as [OM Date] from report_LOC where 1 = 1 
                    END
            
GO
/****** Object:  StoredProcedure [dbo].[report_Operative_LOCs]    Script Date: 4/15/2020 4:17:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

                    
                    CREATE PROCEDURE [dbo].[report_Operative_LOCs]
                    @LOC_Region varchar(max) = '', @LOC_Country varchar(max) = ''
                    AS
                    BEGIN      
					
					


 select 
R.Name [Region], C.Name [Country], L.Name [Borrower], Tenure [Tenor (Months)], FORMAT(TotalAmount/ 1000000, ',0.00') [Amount of Credit (USD Mn)], P.Name [Projects Covered],  FORMAT(P.ProjectValue/ 1000000, ',0.00') [Project Value (USD Mn)]
from TBL_Regions R left join TBL_Country C on R.Id = C.RegionId left join TBL_LOC L on C.Id = L.CountryId left join TBL_LOC_Project LP on L.Id = LP.LocId left join TBL_Projects P on LP.ProjectId = P.Id 
where (isnull(@LOC_Region,'') = '' or R.[Name] like '%'+@LOC_Region+'%') 
 and (isnull(@LOC_Country,'') = '' or C.[Name] like '%'+@LOC_Country+'%') 
order by Region, Country, Borrower




                    END

            
GO
/****** Object:  StoredProcedure [dbo].[report_PQ_Process_Status]    Script Date: 4/15/2020 4:17:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

                    
                    Create PROCEDURE [dbo].[report_PQ_Process_Status]
                    @LOC_Country varchar(max) = '', @Project_Sector varchar(max) = '', @PQ_Category varchar(max) = '', @PQ_Status varchar(max) = '', @PQ_ApplicationStart_From datetime = null,  @PQ_ApplicationStart_To datetime = null, @PQ_LastSubmissionOn_From datetime = null,  @PQ_LastSubmissionOn_To datetime = null
                    AS
                    BEGIN                    
                        select distinct  [LOC_Country] as [LOC_Country],[PQ_LocAmount] as [PQ_LocAmount],[Project_Name] as [Project_Name],[Project_Sector] as [Project_Sector],[Project_ProjectValue] as [Project_ProjectValue],[PQ_Category] as [PQ_Category],[PQ_NoOfPackage] as [PQ_NoOfPackage],[PQ_ApplicationStart] as [PQ_ApplicationStart],[PQ_LastSubmissionOn] as [PQ_LastSubmissionOn],[PQ_Status] as [PQ_Status],[Applicant_Name] as [Applicant_Name],[Applicant_Org] as [Applicant_Org],[Applicant_Status] as [Applicant_Status] from report_PQDetails where (isnull(@LOC_Country,'') = '' or [LOC_Country] like '%'+@LOC_Country+'%') 
 and (isnull(@Project_Sector,'') = '' or [Project_Sector] like '%'+@Project_Sector+'%') 
 and (isnull(@PQ_Category,'') = '' or [PQ_Category] like '%'+@PQ_Category+'%') 
 and (isnull(@PQ_Status,'') = '' or [PQ_Status] like '%'+@PQ_Status+'%') 
 and (@PQ_ApplicationStart_From is null or @PQ_ApplicationStart_From <= [PQ_ApplicationStart]) and (@PQ_ApplicationStart_To is null or @PQ_ApplicationStart_To >= [PQ_ApplicationStart])  
 and (@PQ_LastSubmissionOn_From is null or @PQ_LastSubmissionOn_From <= [PQ_LastSubmissionOn]) and (@PQ_LastSubmissionOn_To is null or @PQ_LastSubmissionOn_To >= [PQ_LastSubmissionOn])  
                    END

            
GO
/****** Object:  StoredProcedure [dbo].[report_Project_Details]    Script Date: 4/15/2020 4:17:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

                    
                    Create PROCEDURE [dbo].[report_Project_Details]
                    
                    AS
                    BEGIN                    
                        select distinct  [LOC_MdAppDate] as [LOC_MdAppDate],[LOC_Region] as [LOC_Region],[LOC_Country] as [LOC_Country],[Project_Address] as [Project_Address],[Project_Code] as [Project_Code],[Project_Name] as [Project_Name],[Project_ProjectValue] as [Project_ProjectValue],[Project_Sector] as [Project_Sector],[Project_SubSectorName] as [Project_SubSectorName],[Project_ProjectStart] as [Project_ProjectStart],[Project_ProjectEnd] as [Project_ProjectEnd],Sum([Contract_EstimateValue]) as [Sum(Contract_EstimateValue)],Sum([Contract_Disbursed Amount]) as [Sum(Contract_Disbursed Amount)],Max([Contract_FACDate]) as [Max(Contract_FACDate)] from report_All where 1 = 1 group by [LOC_MdAppDate],[LOC_Region],[LOC_Country],[Project_Address],[Project_Code],[Project_Name],[Project_ProjectValue],[Project_Sector],[Project_SubSectorName],[Project_ProjectStart],[Project_ProjectEnd]
                    END

            
GO
/****** Object:  StoredProcedure [dbo].[report_Region_wise_LOC_Details]    Script Date: 4/15/2020 4:17:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

                    
CREATE PROCEDURE [dbo].[report_Region_wise_LOC_Details]
@LOC_Region varchar(max) = ''
AS
BEGIN                    
                        
						
					
					select [Region], [No. of Countries], [No.of LOC approved by GOI], [Amount of LOC approved by GOI (USD Mn)], [No. of Signed LOC], [Amount of Signed LOC Agreements (USD Mn)], [Amount of Contracts (USD Mn)], 
					[Amount of Disbursements (USD Mn)], [Amount of Repayments (USD Mn)]

					from (					
					select R.Id as RegionID, R.[Name] as [Region], count(C.Id) [No. of Countries] from TBL_Regions R inner join TBL_Country C on R.Id = C.RegionId group by R.Id, R.Name) A
					left join 
					(select C.RegionId, Count(L.LocAccountNo) [No.of LOC approved by GOI], SUM(L.TotalAmount)/1000000 [Amount of LOC approved by GOI (USD Mn)] from TBL_LOC L inner join TBL_Country C on L.CountryId = C.Id
					where DeaDate is not null group by C.RegionId) B on A.RegionID = B.RegionId
					left join (
					select C.RegionId, Count(L.LocAccountNo) [No. of Signed LOC], SUM(L.TotalAmount)/1000000 [Amount of Signed LOC Agreements (USD Mn)] from TBL_LOC L inner join TBL_Country C on L.CountryId = C.Id
					where SigningDate is not null group by C.RegionId
					) C on A.RegionID = C.RegionId
					left join (
					select CN.RegionId, SUM(C.EstimateValue)/1000000 [Amount of Contracts (USD Mn)] from TBL_Contracts C inner join TBL_Projects_PQ PQ on C.PQId = PQ.Id 
					inner join TBL_LOC_Project LP on PQ.ProjectId = LP.ProjectId inner join TBL_LOC L on LP.LocId = L.Id 
					inner join TBL_Country CN on L.CountryId = CN.Id group by CN.RegionId
					) D on A.RegionID = D.RegionId
					left join (
					select RegionId, SUM([Disbursed Amount])/1000000 [Amount of Disbursements (USD Mn)], SUM([Amount Repaid])/1000000 [Amount of Repayments (USD Mn)] from
					(select C.RegionId, 
					(isnull((select SUM(DisbAmount) from Finacle_Disbursement where FORACID = L.LocAccountNo),0)) [Disbursed Amount],
					(isnull((select SUM(PrincipalCollection) from  Finacle_LocFinancials where FORACID = L.LocAccountNo),0)) [Amount Repaid]
					from TBL_LOC L inner join TBL_Country C on L.CountryId = C.Id ) X 
					group by RegionId
					) E on A.RegionID = E.RegionId
					
					where (isnull(@LOC_Region,'') = '' or [Region] like '%'+@LOC_Region+'%') 



END

            
GO
/****** Object:  StoredProcedure [dbo].[report_Repaid_LOCs]    Script Date: 4/15/2020 4:17:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

                    
                    CREATE PROCEDURE [dbo].[report_Repaid_LOCs]
                    @LOC_Region varchar(max) = '',@LOC_Country varchar(max) = '',
					@LOC_AllocatedAmount_From float = null,@LOC_AllocatedAmount_To float = null,
					@LOC_DeaDate_From datetime = null,@LOC_DeaDate_To datetime = null,
					@LOC_SigningDate_From datetime = null,@LOC_SigningDate_To datetime = null
                    AS
                    BEGIN                    
                        select distinct (case when isdate([LOC_DeaDate])=1 then
                                        (case when month([LOC_DeaDate]) < 4 then convert(varchar(4), year([LOC_DeaDate]) - 1) + '-' + convert(varchar(4), year([LOC_DeaDate]))
                                            when month([LOC_DeaDate]) >= 4 then convert(varchar(4), year([LOC_DeaDate])) + '-' + convert(varchar(4), year([LOC_DeaDate]) + 1)
                                        else '' end)
                                        else ''
                                        end) FinancialYear, [LOC_Region] as [LOC_Region],[LOC_Country] as [LOC_Country],[LOC_Name] as [LOC_Name],
										[LOC_DeaDate] as [LOC_DeaDate],[LOC_MEAAppDate] as [LOC_MEAAppDate],[LOC_AllocatedAmount] as [LOC_AllocatedAmount],
										[LOC_Purpose] as [LOC_Purpose],[LOC_SigningDate] as [LOC_SigningDate],[Contract_EstimateValue] as [Contract_EstimateValue],
										[LOC_Disbursed Amount] as [LOC_Disbursed Amount],[LOC_Amount Repaid] as [LOC_Amount Repaid],[LOC_VerificationNote] as [LOC_VerificationNote] 
										from report_All where 
										([LOC_Disbursed Amount] is null or [LOC_Amount Repaid] is null or  [LOC_Disbursed Amount] - [LOC_Amount Repaid] <=0)
										and (isnull(LOC_Region,'') = '' or [LOC_Region] like '%'+@LOC_Region+'%') and (isnull(LOC_Country,'') = '' or [LOC_Country] like '%'+@LOC_Country+'%') 
										and (@LOC_AllocatedAmount_From is null or [LOC_AllocatedAmount] >= @LOC_AllocatedAmount_From) 
										and (@LOC_AllocatedAmount_To is null or [LOC_AllocatedAmount] <= @LOC_AllocatedAmount_To) 
										and (@LOC_DeaDate_From is null or [LOC_DeaDate] >= @LOC_DeaDate_From ) 
										and (@LOC_DeaDate_To is null or [LOC_DeaDate] <= @LOC_DeaDate_To ) 
										and (@LOC_SigningDate_From is null or [LOC_SigningDate] >= @LOC_SigningDate_From) 
										and (@LOC_SigningDate_To is null or [LOC_SigningDate] <= @LOC_SigningDate_To) 
                    END
GO
/****** Object:  StoredProcedure [dbo].[report_Status_Statement_as_on_28-02-2020]    Script Date: 4/15/2020 4:17:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

                    
                    CREATE PROCEDURE [dbo].[report_Status_Statement_as_on_28-02-2020]
                    @LOC_Region varchar(max) = '', @LOC_Country varchar(max) = '', @LOC_MEAAppDate_From datetime = null,  @LOC_MEAAppDate_To datetime = null, @LOC_SigningDate_From datetime = null,  @LOC_SigningDate_To datetime = null
                    AS
                    BEGIN                    
                        select distinct (case when isdate([LOC_DeaDate])=1 then
                                        (case when month([LOC_DeaDate]) < 4 then convert(varchar(4), year([LOC_DeaDate]) - 1) + '-' + convert(varchar(4), year([LOC_DeaDate]))
                                            when month([LOC_DeaDate]) >= 4 then convert(varchar(4), year([LOC_DeaDate])) + '-' + convert(varchar(4), year([LOC_DeaDate]) + 1)
                                        else '' end)
                                        else ''
                                        end) FinancialYear, [LOC_Region] as [LOC_Region],[LOC_Country] as [LOC_Country],[LOC_Name] as [LOC_Name],[LOC_DeaDate] as [LOC_DeaDate],[LOC_MEAAppDate] as [LOC_MEAAppDate],[LOC_Sanctioned Amount] as [LOC_Sanctioned Amount],[LOC_Purpose] as [LOC_Purpose],[LOC_SigningDate] as [LOC_SigningDate],[LOC_Disbursed Amount] as [LOC_Disbursed Amount],[LOC_Amount Repaid] as [LOC_Amount Repaid],[LOC_VerificationNote] as [LOC_VerificationNote],Sum([Contract_Sanctioned Amount]) as [Sum(Contract_Sanctioned Amount)] from report_All where (isnull(@LOC_Region,'') = '' or [LOC_Region] like '%'+@LOC_Region+'%') 
 and (isnull(@LOC_Country,'') = '' or [LOC_Country] like '%'+@LOC_Country+'%') 
 and (@LOC_MEAAppDate_From is null or @LOC_MEAAppDate_From <= [LOC_MEAAppDate]) and (@LOC_MEAAppDate_To is null or @LOC_MEAAppDate_To >= [LOC_MEAAppDate])  
 and (@LOC_SigningDate_From is null or @LOC_SigningDate_From <= [LOC_SigningDate]) and (@LOC_SigningDate_To is null or @LOC_SigningDate_To >= [LOC_SigningDate])  group by [LOC_Region],[LOC_Country],[LOC_Name],[LOC_DeaDate],[LOC_Sanctioned Amount],[LOC_Purpose],[LOC_SigningDate],[LOC_MEAAppDate],[LOC_Disbursed Amount],[LOC_Amount Repaid],[LOC_VerificationNote]
                    END

GO

/****** Object:  StoredProcedure [dbo].[report_Unutilized_Contracts]    Script Date: 4/15/2020 4:17:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE Procedure [dbo].[report_Unutilized_Contracts]

as

begin

select 
OmNumber [File No.],
Name [Borrower],
SigningDate [Date of Sanction],
[Contract Value]/1000000 as [Value of Contracts Covered under LOC (USD Mn)],
Sanctioned /1000000 [Amount Sanctioned (USD Mn)],
Disbursed/1000000 [Amount Disbursed (USD Mn)],
[Unutilized]/1000000 as [Unutilized Contracts (USD Mn)] 
from TBL_LOC LOC 
left join (

select LocId, SUM(EstimateValue) [Contract Value], SUM(Sanctioned) Sanctioned, SUM(Disbursed) Disbursed,SUM(Unutilized) Unutilized from (
select C.Id,C.EstimateValue, LP.LocId, CT.*, EstimateValue-Disbursed [Unutilized] from TBL_Contracts C left join TBL_Projects_PQ PQ on C.PQId = PQ.Id left join TBL_Projects P on PQ.ProjectId = P.Id left join TBL_LOC_Project LP on P.Id = LP.ProjectId
			left join (select AccountId, MAX(SanctionedAmount) Sanctioned, MAX(CummulativeDebit) Disbursed from Finacle_Contract_Transanctions group by AccountId) CT on CT.AccountId = C.CGSId
			) X group by LocId

) FIN on FIN.LocId = LOC.Id

end;
GO
/****** Object:  StoredProcedure [dbo].[report_Unutilized_Sanctions]    Script Date: 4/15/2020 4:17:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE Procedure [dbo].[report_Unutilized_Sanctions]

as

begin

select 
OmNumber [File No.],
Name [Borrower],
SigningDate [Date of Sanction],
TotalAmount [LOC Value],
Sanctioned [Amount Sanctioned],
Disbursed [Amount Disbursed],
(TotalAmount - Disbursed) [Unutilized] 
from TBL_LOC LOC 
left join (select FORACID, sum(disbamount) Disbursed, MAX(SanctionLimit) Sanctioned 
			from Finacle_Disbursement group by FORACID) FD on ltrim(rtrim(LOC.LocAccountNo)) = ltrim(rtrim(FD.FORACID))

end;
GO
/****** Object:  StoredProcedure [dbo].[report_Year_Wise_Disbursements]    Script Date: 4/15/2020 4:17:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE Procedure [dbo].[report_Year_Wise_Disbursements] --'Asia'
@region varchar(200) = NULL,
@country varchar(200) = NULL 
as

begin

                        
DECLARE @cols AS NVARCHAR(MAX),
    @query  AS NVARCHAR(MAX)

select @cols = STUFF((SELECT ',' + QUOTENAME(TYear) 
                    from (
					
select * from 
(select ProjectId, TYear, SUM(Disbursement) Disbursement from
(select CON.PQId, CGSId, TYear, Disbursement from TBL_Contracts CON left Join (
select AccountId, dbo.FiscalYear(TranDate) TYear, Sum(TranAmount) Disbursement
from Finacle_Contract_Transanctions group by AccountId, dbo.FiscalYear(TranDate))
DISB on CON.CGSId = DISB.AccountId) X inner join TBL_Projects_PQ PQ on X.PQid = PQ.Id 
group by ProjectId, TYear
) Y --where TYear is not null
--order by ProjectId, TYear

							) YT
                    group by TYear
                    order by TYear
            FOR XML PATH(''), TYPE
            ).value('.', 'NVARCHAR(MAX)') 
        ,1,1,'')

set @query = '
select * from (
select 
P.Code as [Project Code],
P.Name as [Project Name], 
R.Name as [Region],
C.Name as [Country],
P.ProjectValue,
'+  @cols  +',
(select Name from Tbl_Status where Id = P.Status) [ProjectStatus],
P.ProjectStart,
P.ProjectEnd

 from (

			SELECT ProjectId,' + @cols + ' from 
             (
			 select * from (
					select ProjectId, TYear, Disbursement
					from (select ProjectId, TYear, SUM(Disbursement) Disbursement from
					(select CON.PQId, CGSId, TYear, Disbursement from TBL_Contracts CON left Join (
					select AccountId, dbo.FiscalYear(TranDate) TYear, Sum(TranAmount) Disbursement
					from Finacle_Contract_Transanctions group by AccountId, dbo.FiscalYear(TranDate))
					DISB on CON.CGSId = DISB.AccountId) X inner join TBL_Projects_PQ PQ on X.PQid = PQ.Id 
					group by ProjectId, TYear
					) Y --where TYear is not null
				) YT
            ) x
            pivot 
            (
                sum(Disbursement)
                for TYear in (' + @cols + ')
            ) p 
			
			) X inner join TBL_Projects P on X.ProjectId = P.Id
			left join TBL_LOC_Project LP on P.Id = LP.ProjectId
			left join TBL_LOC L on L.Id = LP.LOCId
			left join TBL_Country C on C.Id = L.CountryId
			left join TBL_Regions R on R.Id = C.RegionId ) X

			Where 1 = 1 

			'


if(isnull(@region,'') <> '')
begin
set @query += ' and Region like ''%'+@region+'%'' '
end
if(isnull(@country,'') <> '')
begin
set @query += 'and Country like ''%'+@country+'%'' '
end



execute(@query);


--select * from TBL_Regions
--select * from TBL_LOC

end;
GO
/****** Object:  StoredProcedure [dbo].[standard_report_Completed_Projects]    Script Date: 4/15/2020 4:17:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

                    
                    CREATE PROCEDURE [dbo].[standard_report_Completed_Projects]
                    @region varchar(200) = NULL,
					@country varchar(200) = NULL
                    AS
                    BEGIN                    
                        select distinct  Row_Number() over (order by(select(1))) as [Sr.no.],
										 [LOC_Region] as [Region],
										 [LOC_Country] as [Country],
										 Project_Code [Project Code],
										 [Project_Name] as [Project Name],
										 FORMAT([Project_ProjectValue],'#,##0.00') as [Project Value(USD mn)],
										 CONVERT(VARCHAR,[Contract_ContractApprovalDate],103) as [Date of Project Approval],
										 CONVERT(VARCHAR,[Contract_ScheduledCompDate],103) as [Scheduled Completion Date],
										 CONVERT(VARCHAR,[Contract_ActualCompletionDate],103) as [Actual Completion Date] from report_All 
										 where 1 = 1 and 
										 (isnull(@region,'') = '' or LOC_Region in (select * from dbo.SplitString(@region,','))) and
									      (isnull(@country,'') = '' or LOC_Country in (select * from dbo.SplitString(@country,',')))
                    END

            
GO
/****** Object:  StoredProcedure [dbo].[standard_report_Contract_Details]    Script Date: 4/15/2020 4:17:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

                    
                    CREATE PROCEDURE [dbo].[standard_report_Contract_Details]
                     @region varchar(200) = NULL,
					@country varchar(200) = NULL
                    AS
                    BEGIN                    
                        select distinct  Row_Number() over (order by(select(1))) as [Sr.no.],
										 [LOC_Region] as [Region],
										 [LOC_Country] as [Country name/Borrower],
										 [Project_Address] as [Project Location],
										 [Project_Name] as [Project Name],
										 [Contract_ContractorName] as [Name of Contractor],
										 CONVERT(VARCHAR,[Contract_ContractApprovalDate],103) as [Date of Contract Approval],
										 FORMAT([Project_ProjectValue],'#,##0.00') as [Project Value (USD)],
										 FORMAT([Contract_EstimateValue],'#,##0.00') as [Contract Value (USD)],
										 [Project_Sector] as [Sector],
										 [Contract_ContractType] as [Type of Contract CS/EPC/CC/SUP/PLY/LIE],
										 CONVERT(VARCHAR,[Contract_ScheduledCompDate],103) as [Scheduled Completion Date],
										 CONVERT(VARCHAR,[Contract_RevisedCompletionDate],103) as [Revised Completion Date],
										 CONVERT(VARCHAR,[Contract_PACDate],103) as [Date of Provisional Acceptacnce Certificate],
										 CONVERT(VARCHAR,[Contract_FACDate],103) as [Date of Final Acceptacnce Certificate],
										 [Project_Progress] as [Physical Progress],
										 [Project_FinancialProgress] as [Financial Progress],
										 [LOC_IndianContribution] as [Indian Content Requirement(%)],
										 [Contract_Note] as [Contract_Note] from report_All where 1 = 1 AND
										 (isnull(@region,'') = '' or LOC_Region in (select * from dbo.SplitString(@region,','))) and
									      (isnull(@country,'') = '' or LOC_Country in (select * from dbo.SplitString(@country,',')))
                    END

            
GO
/****** Object:  StoredProcedure [dbo].[standard_report_GOI_Guarantees_Issued]    Script Date: 4/15/2020 4:17:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[standard_report_GOI_Guarantees_Issued]
                    @country varchar(max) = ''
                    AS
                    BEGIN                    
                        
						
						select distinct  
						Row_Number() over (order by(select(1))) as [Sr.no.],
						[Name] as [Country/Borrower],
						[LocNumber] as [LocNumber],
						LocAccountNo [Account Number], 
						FORMAT([AmountAllocated]/1000000,'#,##0.00') as [LOC Amount (USD Mn)],
						[Purpose] as [Purpose],
						CONVERT(VARCHAR,[OM Date],103) as [Date of GOI Approval],
						CONVERT(VARCHAR,[SigningDate],103) as [Date of Signing of LOC],
						[OmNumber] as [OM Number] 
						from report_LOC where 1 = 1 and
						(isnull(@country,'') = '' or [Name] IN (select * from dbo.SplitString(@country, ',')))
                    
					
					
					END
GO
/****** Object:  StoredProcedure [dbo].[standard_report_LOC_Commitments]    Script Date: 4/15/2020 4:17:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[standard_report_LOC_Commitments]
                    @country varchar(max) = ''
                    AS
                    BEGIN                    
                        select distinct 
						Row_Number() over (order by(select(1))) as [Sr.no.],
						LocNumber [LOC Number],
						 LocAccountNo [Account Number],
						  [Name] as [Borrower],
						  [Country] as [Country],
						  CONVERT(VARCHAR,[OM Date],103) as [Guaranteed by GOI],
						  FORMAT([AmountAllocated]/1000000,'#,##0.00') as [LOC Amount (USD Mn)] 
						  from report_LOC where 1 = 1 and
						(isnull(@country,'') = '' or [Country] IN (select * from dbo.SplitString(@country, ',')))
                    END
GO
/****** Object:  StoredProcedure [dbo].[standard_report_LOC_Details]    Script Date: 4/15/2020 4:17:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 
                    CREATE PROCEDURE [dbo].[standard_report_LOC_Details]

					@region varchar(200) = NULL,

					@country varchar(200) = NULL
                    AS
                    BEGIN                    
                        select distinct 
						Row_Number() over (order by(select(1))) as [Sr.no.],
						(case when isdate([OM Date])=1 then
                                        (case when month([OM Date]) < 4 then convert(varchar(4), year([OM Date]) - 1) + '-' + convert(varchar(4), year([OM Date]))
                                            when month([OM Date]) >= 4 then convert(varchar(4), year([OM Date])) + '-' + convert(varchar(4), year([OM Date]) + 1)
                                        else '' end)
                                        else ''
                                        end) [Financial year of approval(based on date of first OM from GOI)],
										 [Region] as [Region],
										 [Country] as [Country],
										 [Name] as [Borrower],
										 CONVERT(VARCHAR,[MdAppDate],103) as [Date of approval by DEA],
										  CONVERT(VARCHAR,[MEAAppDate],103) as [Date of MEA's go ahead],
										 FORMAT([AmountAllocated],'#,##0.00') as [LOC Amount(USD mn)],
										 [Purpose] as [Purpose of LOC],
										 CONVERT(VARCHAR,[SigningDate],103) as [Daye of Signing of LOC by the recipient with EXIM bank],
										 FORMAT([Disbursed Amount],'#,##0.00') as [Disbursements so far by exim bank],
										 FORMAT([Amount Repaid],'#,##0.00') as [Principal Repayments made by the borrower(USD mn)],
										 [LOC Status] as [LOC Status],
										 CONVERT(VARCHAR,[OM Date],103) as [OM Date] 
										 from report_LOC where 1 = 1 and
										  (isnull(@region,'') = '' or Region in (select * from dbo.SplitString(@region,','))) and

									 (isnull(@country,'') = '' or Country in (select * from dbo.SplitString(@country,',')))
                    END

            
GO
/****** Object:  StoredProcedure [dbo].[standard_report_Monthly_Status]    Script Date: 4/15/2020 4:17:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/****** Object:  StoredProcedure [dbo].[standard_report_Monthly_Status]    Script Date: 3/29/2020 11:57:27 AM ******/

  CREATE PROCEDURE [dbo].[standard_report_Monthly_Status]
    @month datetime = null
	AS
    BEGIN                 
	
	select Row_Number() over (order by(select(1))) as [Sr.no.], * from (
	   
         select distinct 
						 (case when isdate(r.LOC_DeaDate)=1 then
                         (case when month(r.[LOC_DeaDate]) < 4 then convert(varchar(4), year(r.[LOC_DeaDate]) - 1) + '-' + convert(varchar(4), year(r.[LOC_DeaDate]))
                          when month(r.[LOC_DeaDate]) >= 4 then convert(varchar(4), year(r.[LOC_DeaDate])) + '-' + convert(varchar(4), year(r.[LOC_DeaDate]) + 1)
                          else '' end)
                          else ''
                end) FinancialYear, r.[LOC_Region] as [Region],r.[LOC_Country] as [Country],r.[LOC_Name] as [Borrower],
				LOC_LOCAccountNo [LOC Account Number],
				r.[LOC_DeaDate] as [Date of Approval by DEA],r.[LOC_MEAAppDate] as [Date of MEA's go ahead],
				mfd.LOC_SanctionedAmount as [LOC Amount(USD mn)],r.[LOC_Purpose] as [Purpose of LOC],
				r.[LOC_SigningDate] as [Date of signing of LOC by recepient of EXIM bank],
				mfd.Contract_SanctionedAmount as [Value of Contracts covered under the LOC by EXIM bank(USD mn)],
				mfd.LOC_DisbursedAmount as [Disbursement so far by EXIM bank(USD mn)],
				mfd.LOC_AmountRepaid as [Principal repayment made by the Borrower(USD mn)]
				from report_All r inner join dbo.MonthlyFinacleData mfd on r.LOC_LocNumber = mfd.LOC_LocNumber 
				where (@month is null or mfd.DataMonth = @month) ) X
  END

            
GO
/****** Object:  StoredProcedure [dbo].[standard_report_Operative_LOCs]    Script Date: 4/15/2020 4:17:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 CREATE PROCEDURE [dbo].[standard_report_Operative_LOCs]

                    @region varchar(max) = '', 
					@country varchar(max) = ''

                    AS

                    BEGIN     

select
Row_Number() over (order by(select(1))) as [Sr.no.],
R.Name [Region],
 C.Name [Country], 
 L.Name [Borrower], 
 Tenure [Tenor (Months)], 
 FORMAT(TotalAmount/ 1000000, ',0.00') [Amount of Credit (USD Mn)], P.Name [Projects Covered],  FORMAT(P.ProjectValue/ 1000000, ',0.00') [Project Value (USD Mn)]

from TBL_Regions R left join TBL_Country C on R.Id = C.RegionId left join TBL_LOC L on C.Id = L.CountryId left join TBL_LOC_Project LP on L.Id = LP.LocId left join TBL_Projects P on LP.ProjectId = P.Id

where (isnull(@region,'') = '' or R.[Name] in (select * from dbo.SplitString(@region,',')))

 and (isnull(@country,'') = '' or C.[Name] in (select * from dbo.SplitString(@country,',')))

order by Region, Country, Borrower

 

 

 

 

                    END
GO
/****** Object:  StoredProcedure [dbo].[standard_report_PQ_Process_Status]    Script Date: 4/15/2020 4:17:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
          
                    CREATE PROCEDURE [dbo].[standard_report_PQ_Process_Status]
                    @LOC_Country varchar(max) = '', 
					@Project_Sector varchar(max) = '', 
					@PQ_Category varchar(max) = '', 
					@PQ_Status varchar(max) = '', 
					@PQ_ApplicationStart_From datetime = null,  
					@PQ_ApplicationStart_To datetime = null, 
					@PQ_LastSubmissionOn_From datetime = null,  
					@PQ_LastSubmissionOn_To datetime = null
                    AS
                    BEGIN                    
                        select distinct  Row_Number() over (order by(select(1))) as [Sr.no.],
						[LOC_Country] as [Country Name],
						FORMAT([PQ_LocAmount],'#,##0.00') as [Loc Amount (USD mn)],
						[Project_Name] as [Project Name],
						[Project_Sector] as [Sector],
						FORMAT([Project_ProjectValue],'#,##0.00') as [ProjectValue for which PQ is being/was undertaken (USD mn)],
						[PQ_Category] as [Category],
						[PQ_NoOfPackage] as [No Of Packages],
						CONVERT(VARCHAR,[PQ_ApplicationStart],103) as [Application Start Date (Date of publishing on EXIM's bank website)],
						CONVERT(VARCHAR,[PQ_LastSubmissionOn],103) as [Last Date for Submission],
						[PQ_Status] as [Status],
						[Applicant_Name] as [names of Applicants],
						[Applicant_Org] as [Names of pre Qualified Applicants],
						[Applicant_Status] as [Applicant_Status] 
						from report_PQDetails where 
						(isnull(@LOC_Country,'') = '' or LOC_Country in (select * from dbo.SplitString(@LOC_Country,','))) 
					and (isnull(@Project_Sector,'') = '' or Project_Sector in (select * from dbo.SplitString(@Project_Sector,',')))
					and (isnull(@PQ_Category,'') = '' or [PQ_Category] in (select * from dbo.SplitString(@PQ_Category,','))) 
					and (isnull(@PQ_Status,'') = '' or [PQ_Status] in (select * from dbo.SplitString(@PQ_Status,',')))
					and (@PQ_ApplicationStart_From is null or @PQ_ApplicationStart_From <= isnull([PQ_ApplicationStart],'1900-01-01')) and (@PQ_ApplicationStart_To is null or @PQ_ApplicationStart_To >= isnull([PQ_ApplicationStart],'2050-01-01'))  
					and (@PQ_LastSubmissionOn_From is null or @PQ_LastSubmissionOn_From <= [PQ_LastSubmissionOn]) and (@PQ_LastSubmissionOn_To is null or @PQ_LastSubmissionOn_To >= [PQ_LastSubmissionOn])  
                    END

            
GO
/****** Object:  StoredProcedure [dbo].[standard_report_Project_Details]    Script Date: 4/15/2020 4:17:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

                    
                    CREATE PROCEDURE [dbo].[standard_report_Project_Details]
                    @region varchar(200) = NULL,
					@country varchar(200) = NULL
                    AS
                    BEGIN                    
                        select distinct  Row_Number() over (order by(select(1))) as [Sr.no.],
										 [LOC_MdAppDate] as [Date of Approval by DEA],
										 [LOC_Region] as [Region],
										 [LOC_Country] as [Country Name],
										 [Project_Address] as [Project Location],
										 [Project_Name] as [Project Name],
										 [Project_ProjectValue] as [Project Value (USD mn)],
										 [Project_Sector] as [Sector],
										 [Project_SubSectorName] as [Sub Sector],
										 Sum([Contract_EstimateValue]) as [Contracts Value(USD mn)],
										 Sum([Contract_Disbursed Amount]) as [Disbursement Value(USD mn)],
										 Max([Contract_FACDate]) as [Actual Completion Date of Project] 
										 from report_All where 1 = 1 and
										 (isnull(@region,'') = '' or LOC_Region in (select * from dbo.SplitString(@region,','))) and
									      (isnull(@country,'') = '' or LOC_Country in (select * from dbo.SplitString(@country,',')))
										 group by [LOC_MdAppDate],[LOC_Region],[LOC_Country],[Project_Address],
										 [Project_Name],[Project_ProjectValue],[Project_Sector],[Project_SubSectorName]
                    END

            
GO
/****** Object:  StoredProcedure [dbo].[standard_report_Region_wise_LOC_Details]    Script Date: 4/15/2020 4:17:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[standard_report_Region_wise_LOC_Details]
                    @LOC_Region varchar(max) = ''
                    AS
                    BEGIN                    
                        select distinct Row_Number() over (order by(select(1))) as [Sr.no.],  [LOC_Region] as [Region],Count(distinct [LOC_Country]) as [No. of Countries],
						Count( distinct [LOC_LocNumber]) as [No. of approved by GOI],
						FORMAT(Sum([LOC_AllocatedAmount])/1000000,'#,##0.00') as [Amount  of approved by GOI(USD Mn)],
						FORMAT(Sum([Contract_EstimateValue])/1000000,'#,##0.00') as [Amount  of Contracts(USD Mn)],
						FORMAT(Sum([LOC_Disbursed Amount])/1000000,'#,##0.00') as [Amount  of Disbursements(USD Mn)],
						FORMAT(Sum([LOC_Amount Repaid])/1000000,'#,##0.00') as [Amount  of Repayments(USD Mn)] from report_All 
						where (isnull(@LOC_Region,'') = '' or [LOC_Region] IN (select * from dbo.SplitString(@LOC_Region, ','))) group by [LOC_Region]
                    END
GO
/****** Object:  StoredProcedure [dbo].[standard_report_Repaid_LOCs]    Script Date: 4/15/2020 4:17:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

                    
                    CREATE PROCEDURE [dbo].[standard_report_Repaid_LOCs]
                    @LOC_Region varchar(max) = '',
					@LOC_Country varchar(max) = '',
					@LOC_AllocatedAmount varchar(max) = '',
					@LOC_DeaDate_From datetime = null,
					@LOC_DeaDate_To datetime = null,
					@LOC_SigningDate_From datetime = null,
					@LOC_SigningDate_To datetime = null
                    AS
                    BEGIN                    
                        select distinct Row_Number() over (order by(select(1))) as [Sr.no.],
										(case when isdate([LOC_DeaDate])=1 then
                                        (case when month([LOC_DeaDate]) < 4 then convert(varchar(4), year([LOC_DeaDate]) - 1) + '-' + convert(varchar(4), year([LOC_DeaDate]))
                                            when month([LOC_DeaDate]) >= 4 then convert(varchar(4), year([LOC_DeaDate])) + '-' + convert(varchar(4), year([LOC_DeaDate]) + 1)
                                        else '' end)
                                        else ''
                                        end) FinancialYear, 
										[LOC_Region] as [LOC_Region],
										[LOC_Country] as [LOC_Country],
										[LOC_Name] as [LOC_Name],
										CONVERT(VARCHAR,[LOC_DeaDate],103) as [LOC_DeaDate],
										CONVERT(VARCHAR,[LOC_MEAAppDate],103) as [LOC_MEAAppDate],
										FORMAT([LOC_AllocatedAmount],'#,##0.00') as [LOC_AllocatedAmount],
										[LOC_Purpose] as [LOC_Purpose],
										CONVERT(VARCHAR,[LOC_SigningDate],103) as [LOC_SigningDate],
										FORMAT([Contract_EstimateValue],'#,##0.00') as [Contract_EstimateValue],
										FORMAT([LOC_Disbursed Amount],'#,##0.00') as [LOC_Disbursed Amount],
										FORMAT([LOC_Amount Repaid],'#,##0.00') as [LOC_Amount Repaid],
										[LOC_VerificationNote] as [LOC_VerificationNote] 
										from report_All where 
										(isnull(@LOC_Region,'') = '' or [LOC_Region] IN (select * from dbo.SplitString(@LOC_Region, ','))) 
										and (isnull(@LOC_Country,'') = '' or [LOC_Country] IN (select * from dbo.SplitString(@LOC_Country, ',')))
										and (isnull(@LOC_AllocatedAmount,'') = '' or [LOC_AllocatedAmount] IN (select * from dbo.SplitString(@LOC_AllocatedAmount, ','))) 
										and (@LOC_DeaDate_From is null or [LOC_DeaDate] >= @LOC_DeaDate_From ) 
										and (@LOC_DeaDate_To is null or [LOC_DeaDate] <= @LOC_DeaDate_To ) 
										and (@LOC_SigningDate_From is null or [LOC_SigningDate] >= @LOC_SigningDate_From) 
										and (@LOC_SigningDate_To is null or [LOC_SigningDate] <= @LOC_SigningDate_To) 
                    END
GO
/****** Object:  StoredProcedure [dbo].[standard_report_Unutilized_Contracts]    Script Date: 4/15/2020 4:17:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Procedure [dbo].[standard_report_Unutilized_Contracts]
@borrower varchar(max) = ''
as

begin

select 
Row_Number() over (order by(select(1))) as [Sr.no.],
OmNumber [File No.],
Name [Borrower],
CONVERT(VARCHAR,[SigningDate],103) [Date of Sanction],
FORMAT([Contract Value]/1000000,'#,##0.00') as [Value of Contracts Covered under LOC (USD Mn)],
FORMAT(Sanctioned /1000000,'#,##0.00') [Amount Sanctioned (USD Mn)],
FORMAT(Disbursed/1000000,'#,##0.00') [Amount Disbursed (USD Mn)],
FORMAT([Unutilized]/1000000,'#,##0.00') as [Unutilized Contracts (USD Mn)] 
from TBL_LOC LOC 
left join (

select LocId, SUM(EstimateValue) [Contract Value], SUM(Sanctioned) Sanctioned, SUM(Disbursed) Disbursed,SUM(Unutilized) Unutilized from (
select C.Id,C.EstimateValue, LP.LocId, CT.*, EstimateValue-Disbursed [Unutilized] from TBL_Contracts C left join TBL_Projects_PQ PQ on C.PQId = PQ.Id left join TBL_Projects P on PQ.ProjectId = P.Id left join TBL_LOC_Project LP on P.Id = LP.ProjectId
			left join (select AccountId, MAX(SanctionedAmount) Sanctioned, MAX(CummulativeDebit) Disbursed from Finacle_Contract_Transanctions group by AccountId) CT on CT.AccountId = C.CGSId
			) X group by LocId

) FIN on FIN.LocId = LOC.Id
WHERE 1=1 and
(isnull(@borrower,'') = '' or [Name] IN (select * from dbo.SplitString(@borrower, ',')))

end;
GO
/****** Object:  StoredProcedure [dbo].[standard_report_Unutilized_Sanctions]    Script Date: 4/15/2020 4:17:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Procedure [dbo].[standard_report_Unutilized_Sanctions]
@borrower varchar(max) = ''
as

begin

select 
Row_Number() over (order by(select(1))) as [Sr.no.],
OmNumber [File No.],
Name [Borrower],
CONVERT(VARCHAR,[SigningDate],103) [Date of Sanction],
FORMAT(TotalAmount,'#,##0.00') [LOC Value],
FORMAT(Sanctioned,'#,##0.00') [Amount Sanctioned],
FORMAT(Disbursed,'#,##0.00') [Amount Disbursed],
FORMAT((TotalAmount - Disbursed),'#,##0.00') [Unutilized] 
from TBL_LOC LOC 
left join (select FORACID, sum(disbamount) Disbursed, MAX(SanctionLimit) Sanctioned 
			from Finacle_Disbursement group by FORACID) FD on ltrim(rtrim(LOC.LocAccountNo)) = ltrim(rtrim(FD.FORACID))
WHERE 1=1 and
(isnull(@borrower,'') = '' or [Name] IN (select * from dbo.SplitString(@borrower, ',')))
end;
GO
/****** Object:  StoredProcedure [dbo].[standard_report_Year_Wise_Disbursements]    Script Date: 4/15/2020 4:17:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE Procedure [dbo].[standard_report_Year_Wise_Disbursements] --'Asia,Africa'
					
		@region varchar(200) = NULL,
					@country varchar(200) = NULL				
as

begin

                        
DECLARE @cols AS NVARCHAR(MAX),
    @query  AS NVARCHAR(MAX)


select @cols = STUFF((SELECT ',' + QUOTENAME(TYear) 
                    from (
					
select * from 
(select ProjectId, TYear, FORMAT(SUM(Disbursement),'#,##0.00') Disbursement from
(select CON.PQId, CGSId, TYear, Disbursement from TBL_Contracts CON left Join (
select AccountId, dbo.FiscalYear(TranDate) TYear, Sum(TranAmount) Disbursement
from Finacle_Contract_Transanctions group by AccountId, dbo.FiscalYear(TranDate))
DISB on CON.CGSId = DISB.AccountId) X inner join TBL_Projects_PQ PQ on X.PQid = PQ.Id 
group by ProjectId, TYear
) Y --where TYear is not null
--order by ProjectId, TYear

							) YT
                    group by TYear
                    order by TYear
            FOR XML PATH(''), TYPE
            ).value('.', 'NVARCHAR(MAX)') 
        ,1,1,'')

set @query = '
select * from (
select 
P.Code as [Project Code],
P.Name as [Project Name], 
R.Name as [Region],
C.Name as [Country],
FORMAT(P.ProjectValue,''#,##0.00'') as [Project Cost (USD mn)],
'+  @cols  +',
(select Name from Tbl_Status where Id = P.Status) [ProjectStatus],
CONVERT(VARCHAR,P.ProjectStart,103) [Project Start],
CONVERT(VARCHAR,P.ProjectEnd,103) [Project End]

 from (

			SELECT ProjectId,' + @cols + ' from 
             (
			 select * from (
					select ProjectId, TYear, Disbursement
					from (select ProjectId, TYear, SUM(Disbursement) Disbursement from
					(select CON.PQId, CGSId, TYear, Disbursement from TBL_Contracts CON left Join (
					select AccountId, dbo.FiscalYear(TranDate) TYear, Sum(TranAmount) Disbursement
					from Finacle_Contract_Transanctions group by AccountId, dbo.FiscalYear(TranDate))
					DISB on CON.CGSId = DISB.AccountId) X inner join TBL_Projects_PQ PQ on X.PQid = PQ.Id 
					group by ProjectId, TYear
					) Y --where TYear is not null
				) YT
            ) x
            pivot 
            (
                sum(Disbursement)
                for TYear in (' + @cols + ')
            ) p 
			
			) X inner join TBL_Projects P on X.ProjectId = P.Id
			left join TBL_LOC_Project LP on P.Id = LP.ProjectId
			left join TBL_LOC L on L.Id = LP.LOCId
			left join TBL_Country C on C.Id = L.CountryId
			left join TBL_Regions R on R.Id = C.RegionId) X where 1 = 1 '
			
if(isnull(@region,'') <> '')
begin
set @query	+= 	' and Region in (select * from dbo.SplitString('''+@region+''','',''))'
end
if(isnull(@country,'') <> '')
begin
set @query	+= 	'and Country in (select * from dbo.SplitString('''+@country+''','',''))'
end
			
			
			

execute(@query)


--select * from TBL_Regions
--select * from TBL_LOC

end;
GO
/****** Object:  StoredProcedure [dbo].[Update_Project_Contracts]    Script Date: 4/15/2020 4:17:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Procedure [dbo].[Update_Project_Contracts]
@ProjectId int,
@PackageId nvarchar(max),
@PackageName nvarchar(max),
@PackageDisplayId nvarchar(max),
@EstimateValue nvarchar(max),
@TypeOfPackage nvarchar(max),
@PQNo nvarchar(max),
@PQId nvarchar(max),
@Ref nvarchar(max),
@Title nvarchar(max),
@Country nvarchar(max),
@Status nvarchar(max),
@LocNumber nvarchar(max),
@LocAmount nvarchar(max),
@NoOfPackage nvarchar(max)
as
Begin
SET NOCOUNT ON
declare @msg varchar(max);
declare @_status bit;
declare @processedId int;
Declare @NoCount int = 0;
Declare @ProjectPqId int;
--For TBL_Applicants
Declare @tempId int = 0;
Declare @ContractId int = 0;
Declare @ApplicantStatus int;
Declare @Name nvarchar(255);
Declare @Organization nvarchar(255);
Declare @SupplierId nvarchar(255);

  Begin Transaction TR;
  Begin Try
	  Select @NoCount = Count(*) from TBL_Projects_PQ where PqNo = @PQNo

	  if @NoCount = 0 begin
		Insert into TBL_Projects_PQ(PqId, PqNo, Ref,  ProjectId, Country, pq_status, PQRefNumber,
		LocNumber, LocAmount, NoOfPackage) values(@PQId, @PQNo, @Ref, @ProjectId, @Country, @Status, @PQNo,
		@LocNumber, @LocAmount, @NoOfPackage);
		set @ProjectPqId = SCOPE_IDENTITY();
	  end
	  else begin
	  Select @ProjectPqId = Id from TBL_Projects_PQ where  PqNo = @PQNo

	   Update TBL_Projects_PQ set  PqNo = @PQNo , PqId = @PQId, PQRefNumber = @PQNo
	  , Ref = @Ref , Title = @Title , Country = @Country
	  , LocNumber = @LocNumber , LocAmount = @LocAmount
	  , NoOfPackage = @NoOfPackage where PqNo = @PQNo
	  end

	  Set @NoCount= 0;
	  Select @NoCount = Count(*) from TBL_Contracts where PackageId = @PackageId and PackageName = @PackageName
	  and PackageDisplayId = @PackageDisplayId and EstimateValue = @EstimateValue and TypeOfPackage = @TypeOfPackage
	  
	  if @NoCount = 0 begin
		Insert into TBL_Contracts(PQId, PackageId, PackageName, PackageDisplayId, EstimateValue,
		TypeOfPackage) values (@ProjectPqId, @PackageId, @PackageName, @PackageDisplayId, @EstimateValue,
		@TypeOfPackage);
		set @ContractId=SCOPE_IDENTITY();
	  end
	  else begin
		Select @ContractId = Id from TBL_Contracts where PackageId = @PackageId ;
		 Update TBL_Contracts Set PackageName = @PackageName, PackageDisplayId = @PackageDisplayId,
		 EstimateValue = @EstimateValue, TypeOfPackage = @TypeOfPackage where PackageId= @PackageId;
	  end
	 -- Select * into #temp from (Select bs.Id, bs.Status, bv.FirstName, bv.CompanyName, bs.SupplierId 
	 -- From BobEProcure_Status as bs inner join BobEProcure_Vendors as bv on bv.UserId = bs.SupplierId
	 -- where bs.PQId = @PQId) a;
	 -- while exists(Select * from #temp) begin
		--set @NoCount = 0;
		--Select Top 1 @tempId = Id, @ApplicantStatus = Status, @Name = FirstName, @Organization = CompanyName,  
		--@SupplierId = SupplierId from #temp
		--Select @NoCount = Count(*) from Tbl_Applicants where SupplierId = @SupplierId and ContractId = @ContractId;
		--if @NoCount = 0 begin
		--	insert into Tbl_Applicants(ContractId, Status, Name, Organization, SupplierId)
		--	values (@ContractId, @ApplicantStatus, @Name, @Organization, @SupplierId);
		--end
		--else begin
		--	Update Tbl_Applicants Set Status = @Status, Name = @Name, Organization = @Organization
		--	WHERE SupplierId = @SupplierId and ContractId = @ContractId;
		--end
		--Delete from #temp where Id = @tempId;
	 -- end

	  set @processedId = @ProjectPqId;
	  set @msg = 'Data Updated Successfully';
	  set @_status = 1;
	  Commit Transaction TR;

	end TRy
	Begin Catch
		Declare @ErrorMessage NVarChar(4000) 
		set @ErrorMessage=ERROR_MESSAGE();
		set @msg=@ErrorMessage;
		set @_status = 0;
		set @processedId = 0;
		--RaisError(@ErrorMessage, 16, 16);
		ROLLBACK TRANSACTION TR;
	end catch
  
   select @processedId as ProcessedId, @msg as Message, @_status as Status

end
GO
/****** Object:  StoredProcedure [dbo].[Update_ProjectProgress]    Script Date: 4/15/2020 4:17:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[Update_ProjectProgress]
@projectId int,
@statusId int
as
begin
declare @msg varchar(max);
declare @_status bit;
declare @processedId int;
begin try
	update TBL_Projects set Status = @statusId where Id = @projectId;
	set @processedId = @projectId;
	set @msg = 'Project Progress Updated';
	set @_status = 1;
end try
begin catch
	set @processedId = @projectId;
	set @msg = ERROR_MESSAGE();
	set @_status = 0;
end catch
	select @processedId as ProcessedId, @msg as Message, @_status as Status
end;
GO
/****** Object:  StoredProcedure [dbo].[UpdateProject]    Script Date: 4/15/2020 4:17:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[UpdateProject]
@id int,
@name varchar(300),
@description varchar(max),
@status int,
@dprDate datetime,
@baselineData varchar(max),
@locationAddress varchar(300),
@locationCordinates varchar(50),
@stage int,
@subSector int,
@preQualification bit,
@authority varchar(max),
@progress float,
@financialprogress float,
@projectValue decimal(18,2),
@projectStart datetime,
@projectEnd datetime,
@sector varchar(200),
@subsectorname varchar(MAX),
@address varchar(max),
@note varchar(max),
@physicalprogress float
as
begin
	SET NOCOUNT ON
	declare @msg varchar(max);
	declare @_status bit;
	declare @processedId int;
	begin try
		update TBL_Projects set Name =@name, Description = @description,  DPRDate = @dprDate,
		BaselineData =@baselineData, LocationAddress = @locationAddress, LocationCoordinates = @locationCordinates,
		Stage = @stage, SubSector = @subSector, PreQualification =@preQualification, AuthorityName = @authority, 
		Progress = @progress, FinancialProgress =@financialprogress, ProjectValue = @projectValue,
		ProjectStart =@projectStart, ProjectEnd=@projectEnd, Sector = @sector, SubSectorName = @subsectorname, 
		[Address] = @address, Note = @note where Id= @id;

		update TBL_Projects set Progress = @physicalprogress
		--(case when ProjectStart is null then 0 
		--when ProjectEnd is null then 0 
		--when datediff(day, ProjectStart, ProjectEnd) < datediff(day, ProjectStart, getdate()) then 100
		--else ceiling(datediff(day, ProjectStart, getdate())*1.00/datediff(day, ProjectStart, ProjectEnd) * 100) end)
		where Id= @id;

		if exists (select * from TBL_ProjectTimeLines where TimelineTitle = 'Project Start' and ProjectId = @id)
		begin
			update TBL_ProjectTimeLines set TimelineDate = @projectStart where TimelineTitle = 'Project Start' and ProjectId = @id
		end
		else
		begin
			insert into TBL_ProjectTimeLines (ProjectId, TimelineTitle, TimelineDate) select @id, 'Project Start', @projectStart
		end

		if exists (select * from TBL_ProjectTimeLines where TimelineTitle = 'Project End' and ProjectId = @id)
		begin
			update TBL_ProjectTimeLines set TimelineDate = @projectEnd where TimelineTitle = 'Project End' and ProjectId = @id
		end
		else
		begin
			insert into TBL_ProjectTimeLines (ProjectId, TimelineTitle, TimelineDate) select @id, 'Project End', @projectEnd
		end
		set @processedId = @id;
		SET @_status = 1; 
		SET @msg = ''; 
	end try
	begin catch
		set @processedId = 0;
		set @msg = ERROR_MESSAGE();
		set @_status = 0;
	end catch
	select @processedId as ProcessedId, @msg as Message, @_status as Status
end;
GO
/****** Object:  StoredProcedure [System].[Monitoring]    Script Date: 4/15/2020 4:17:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--Create a New SQL Agent to run Execute this stored procedure every 10 minutes.

-- AMP Monitoring System
CREATE PROCEDURE [System].[Monitoring] 
      
AS
BEGIN


----------------------------------------------------------------------------------------------------------------------
-- Update Review dates to remove 1900
----------------------------------------------------------------------------------------------------------------------

--SELECT * FROM Project.Performance WHERE ARRequired='No' AND ARDueDate is not null and ARPromptDate is not null

--BEGIN TRAN
--UPDATE Project.Performance
--SET ARDueDate = null, ARPromptDate = null
--WHERE
--	ARRequired = 'No'

--UPDATE Project.Performance
--SET PCRDueDate = null, PCRPrompt = null
--WHERE
--	PCRRequired = 'No'


--UPDATE Project.Performance
--SET PCRDueDate = null, PCRPrompt = null
--WHERE
--	ProjectID IN (SELECT ProjectID FROM Project.ProjectMaster WHERE Stage = 7)


--UPDATE Project.Performance
--SET ARDueDate = null, ARPromptDate = null
--WHERE
--	ProjectID IN (SELECT ProjectID FROM Project.ProjectMaster WHERE Stage = 7)



--UPDATE Project.Performance
--SET PCRDueDate = null, PCRPrompt = null
--WHERE
--	ProjectID IN (SELECT ProjectID FROM Project.ProjectMaster WHERE Stage <5)


--UPDATE Project.Performance
--SET ARDueDate = null, ARPromptDate = null
--WHERE
--	ProjectID IN (SELECT ProjectID FROM Project.ProjectMaster WHERE Stage <5)
--COMMIT



---- Monitor changes to end dates
--BEGIN TRAN

--UPDATE 
--	Project.Performance 
--SET 
--	PCRDueDate = DATEADD(MONTH,3,pd.OperationalEndDate),
--	PCRPrompt = DATEADD(MONTH,-3,pd.OperationalEndDate)
--FROM 
--	Project.ProjectDates pd
--INNER JOIN 
--	Project.Performance p
--ON 
--	pd.ProjectID = p.ProjectID
--INNER JOIN 
--	Project.ProjectMaster pm
--ON
--	pd.ProjectID = pm.ProjectID
--WHERE 
--	p.PCRRequired = 'Yes'
--AND 
--	pm.Stage = 5
--AND 
--	DATEADD(MONTH,3,pd.OperationalEndDate) != p.PCRDueDate

--COMMIT

-- Monitor for projects going to stage 5 which need a AR
--BEGIN TRAN

--UPDATE 
--	Project.Performance 
--SET 
--	ARDueDate = DATEADD(MONTH,12,pd.ActualStartDate),
--	ARPromptDate = DATEADD(MONTH,9,pd.ActualStartDate)
----	ARRequired ='Yes'
--FROM 
--	Project.ProjectDates pd
--INNER JOIN 
--	Project.Performance p
--ON 
--	pd.ProjectID = p.ProjectID
--INNER JOIN 
--	Project.ProjectMaster pm
--ON
--	pd.ProjectID = pm.ProjectID
--WHERE 
--	pm.Stage = 5
--AND 
--	ARDueDate is null
--AND 
--	ARRequired = 'Yes'
--COMMIT

-- Monitor for projects going to stage 5 which need a AR
--BEGIN TRAN 
--UPDATE 
--	Project.Performance 
--SET 
--	PCRDueDate = DATEADD(MONTH,3,pd.OperationalEndDate),
--	PCRPrompt =  DATEADD(MONTH,-3,pd.OperationalEndDate),
--	PCRRequired = 'Yes'
--FROM 
--	Project.ProjectDates pd
--INNER JOIN 
--	Project.Performance p
--ON 
--	pd.ProjectID = p.ProjectID
--INNER JOIN 
--	Project.ProjectMaster pm
--ON
--	pd.ProjectID = pm.ProjectID
--WHERE 
--	pm.Stage = 5
--AND 
--	PCRDueDate is null
--AND 
--	PCRRequired = 'Yes'

--COMMIT
SELECT ''
END
GO
/****** Object:  StoredProcedure [System].[NightlyTasks]    Script Date: 4/15/2020 4:17:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [System].[NightlyTasks] 
      
AS
BEGIN
BEGIN TRAN
-- No Code...what do we want to sync nightly?
SELECT ''
COMMIT
END


GO
USE [master]
GO
ALTER DATABASE [LMPdev] SET  READ_WRITE 
GO

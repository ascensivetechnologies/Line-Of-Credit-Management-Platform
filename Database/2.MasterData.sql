USE [LMPdev]
GO

SET IDENTITY_INSERT [dbo].[TBL_ContactTypes] ON 
INSERT [dbo].[TBL_ContactTypes] ([Id], [Name], [IsActive]) VALUES (1, N'Office Contact', 1)
INSERT [dbo].[TBL_ContactTypes] ([Id], [Name], [IsActive]) VALUES (2, N'Residence Contact', 1)
SET IDENTITY_INSERT [dbo].[TBL_ContactTypes] OFF

SET IDENTITY_INSERT [dbo].[TBL_Contacts] ON
INSERT [dbo].[TBL_Contacts] ([Id], [Name], [Landline], [Faxno], [MobileNumber], [Email], [AddressLine1], [AddressLine2], [Organization], [Designation], [ContactTypeId], [ContactImg], [IsActive], [City], [PinCode], [CountryId]) VALUES (1, N'LOC', N'23-756565', NULL, N'4577856412', N'test@gmail.com', NULL, NULL, N'Test World', N'LOC Manager', 1, NULL, 1, N'TestMan', N'815412', null)
SET IDENTITY_INSERT [dbo].[TBL_Contacts] OFF


SET IDENTITY_INSERT [dbo].[TBL_Users] ON 
INSERT [dbo].[TBL_Users] ([Id], [Username], [EmployeeNo], [Department], [DisplayName], [EmailAddress], [ContactId]) VALUES (1, N'Birendra', N'-', N'-', N'Display Name', N'email@id.com', 1)
SET IDENTITY_INSERT [dbo].[TBL_Users] OFF

SET IDENTITY_INSERT [dbo].[TBL_Roles] ON 
INSERT [dbo].[TBL_Roles] ([Id], [RoleName]) VALUES (1, N'Admin')
INSERT [dbo].[TBL_Roles] ([Id], [RoleName]) VALUES (2, N'DeskOfficer')
INSERT [dbo].[TBL_Roles] ([Id], [RoleName]) VALUES (3, N'Management')
SET IDENTITY_INSERT [dbo].[TBL_Roles] OFF

SET IDENTITY_INSERT [dbo].[TBL_UserRoleMap] ON 
INSERT [dbo].[TBL_UserRoleMap] ([Id], [UserId], [RoleId]) VALUES (4, 1, 2)
INSERT [dbo].[TBL_UserRoleMap] ([Id], [UserId], [RoleId]) VALUES (5, 1, 1)
SET IDENTITY_INSERT [dbo].[TBL_UserRoleMap] OFF

SET IDENTITY_INSERT [dbo].[TBL_Regions] ON 
INSERT [dbo].[TBL_Regions] ([Id], [Name], [AddedOn], [UpdatedOn], [UpdatedBy], [AddedBy]) VALUES (1, N'Asia', CAST(N'1995-04-30T00:00:00.000' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[TBL_Regions] ([Id], [Name], [AddedOn], [UpdatedOn], [UpdatedBy], [AddedBy]) VALUES (4, N'Africa', CAST(N'2019-05-04T17:27:44.980' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[TBL_Regions] ([Id], [Name], [AddedOn], [UpdatedOn], [UpdatedBy], [AddedBy]) VALUES (5, N'CIS', CAST(N'2019-05-04T18:12:45.517' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[TBL_Regions] ([Id], [Name], [AddedOn], [UpdatedOn], [UpdatedBy], [AddedBy]) VALUES (8, N'LAC', CAST(N'2019-05-06T10:22:15.617' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[TBL_Regions] ([Id], [Name], [AddedOn], [UpdatedOn], [UpdatedBy], [AddedBy]) VALUES (9, N'Oceania', CAST(N'2019-06-17T15:56:56.613' AS DateTime), NULL, NULL, NULL)
SET IDENTITY_INSERT [dbo].[TBL_Regions] OFF

SET IDENTITY_INSERT [dbo].[TBL_Terms] ON 
INSERT [dbo].[TBL_Terms] ([Id], [InterestRate], [CommitmentFee], [ManagementFee], [Tenure], [Moratorium], [IndianContribution], [SpecialConditions], [CurrencyCode], [Type], [ApprovalType], [Percentage], [LOCClassification], [RiskClassification], [AddedBy], [AddedOn], [UpdatedOn], [UpdatedBy], [InterestType]) VALUES (6, 1.75, 0.5, 0.5, 20, 5, 75, N'None', NULL, N'CLL06-Libor', N'DEA', 1.45, N'Cat II as per 2015 Guidelines', N'Insignificant', NULL, NULL, CAST(N'2020-02-11T12:52:21.907' AS DateTime), 1, N'Interest Only')
INSERT [dbo].[TBL_Terms] ([Id], [InterestRate], [CommitmentFee], [ManagementFee], [Tenure], [Moratorium], [IndianContribution], [SpecialConditions], [CurrencyCode], [Type], [ApprovalType], [Percentage], [LOCClassification], [RiskClassification], [AddedBy], [AddedOn], [UpdatedOn], [UpdatedBy], [InterestType]) VALUES (7, 1.5, 0.5, 0.5, 15, 5, 75, N'None', NULL, N'Interest Only', N'DEA', 1.2, N'Cat III as per 2015 Guidelines', N'Insignificant', NULL, NULL, CAST(N'2020-02-11T12:52:03.113' AS DateTime), 1, N'CLL06-Libor')
INSERT [dbo].[TBL_Terms] ([Id], [InterestRate], [CommitmentFee], [ManagementFee], [Tenure], [Moratorium], [IndianContribution], [SpecialConditions], [CurrencyCode], [Type], [ApprovalType], [Percentage], [LOCClassification], [RiskClassification], [AddedBy], [AddedOn], [UpdatedOn], [UpdatedBy], [InterestType]) VALUES (8, 1.5, 0.5, 0, 25, 5, 75, N'None', NULL, N'CLL06-Libor', N'DEA', 1.7, N'Cat I as per 2015 Guidelines', N'Insignificant', 1, CAST(N'2019-09-09T17:51:12.090' AS DateTime), CAST(N'2020-02-11T12:52:26.567' AS DateTime), 1, N'CLL06-Libor')
SET IDENTITY_INSERT [dbo].[TBL_Terms] OFF

SET IDENTITY_INSERT [dbo].[TBL_Options] ON 
INSERT [dbo].[TBL_Options] ([Id], [Type], [Value], [Parent], [AddedBy], [AddedOn], [UpdatedOn], [UpdatedBy], [FaIcon]) VALUES (1, 2, N'Cat I as per 2015 Guidelines', NULL, 1, CAST(N'2019-07-16T13:25:40.070' AS DateTime), CAST(N'2019-11-05T12:57:29.047' AS DateTime), 1, NULL)
INSERT [dbo].[TBL_Options] ([Id], [Type], [Value], [Parent], [AddedBy], [AddedOn], [UpdatedOn], [UpdatedBy], [FaIcon]) VALUES (2, 2, N'Cat II as per 2015 Guidelines', NULL, 1, CAST(N'2019-07-16T13:40:11.757' AS DateTime), CAST(N'2019-11-05T12:57:34.507' AS DateTime), 1, NULL)
INSERT [dbo].[TBL_Options] ([Id], [Type], [Value], [Parent], [AddedBy], [AddedOn], [UpdatedOn], [UpdatedBy], [FaIcon]) VALUES (3, 2, N'Cat III as per 2015 Guidelines', NULL, 1, CAST(N'2019-07-16T13:40:26.667' AS DateTime), CAST(N'2019-11-05T12:57:39.143' AS DateTime), 1, NULL)
INSERT [dbo].[TBL_Options] ([Id], [Type], [Value], [Parent], [AddedBy], [AddedOn], [UpdatedOn], [UpdatedBy], [FaIcon]) VALUES (4, 1, N'Insignificant', NULL, 1, CAST(N'2019-07-16T13:43:20.747' AS DateTime), CAST(N'2019-07-16T13:43:20.747' AS DateTime), 1, NULL)
INSERT [dbo].[TBL_Options] ([Id], [Type], [Value], [Parent], [AddedBy], [AddedOn], [UpdatedOn], [UpdatedBy], [FaIcon]) VALUES (5, 1, N'Low', NULL, 1, CAST(N'2019-07-16T13:25:40.070' AS DateTime), CAST(N'2019-07-16T13:25:40.070' AS DateTime), 1, NULL)
INSERT [dbo].[TBL_Options] ([Id], [Type], [Value], [Parent], [AddedBy], [AddedOn], [UpdatedOn], [UpdatedBy], [FaIcon]) VALUES (6, 1, N'ModeratelyLow', NULL, 1, CAST(N'2019-07-16T13:40:11.757' AS DateTime), CAST(N'2019-07-16T13:40:11.757' AS DateTime), 1, NULL)
INSERT [dbo].[TBL_Options] ([Id], [Type], [Value], [Parent], [AddedBy], [AddedOn], [UpdatedOn], [UpdatedBy], [FaIcon]) VALUES (7, 1, N'Moderate', NULL, 1, CAST(N'2019-07-16T13:40:26.667' AS DateTime), CAST(N'2019-07-16T13:40:26.667' AS DateTime), 1, NULL)
INSERT [dbo].[TBL_Options] ([Id], [Type], [Value], [Parent], [AddedBy], [AddedOn], [UpdatedOn], [UpdatedBy], [FaIcon]) VALUES (8, 1, N'High', NULL, 1, CAST(N'2019-07-16T13:43:20.747' AS DateTime), CAST(N'2019-07-16T13:43:20.747' AS DateTime), 1, NULL)
INSERT [dbo].[TBL_Options] ([Id], [Type], [Value], [Parent], [AddedBy], [AddedOn], [UpdatedOn], [UpdatedBy], [FaIcon]) VALUES (9, 1, N'Restricted**', NULL, 1, CAST(N'2019-07-16T13:40:11.757' AS DateTime), CAST(N'2019-07-16T13:40:11.757' AS DateTime), 1, NULL)
INSERT [dbo].[TBL_Options] ([Id], [Type], [Value], [Parent], [AddedBy], [AddedOn], [UpdatedOn], [UpdatedBy], [FaIcon]) VALUES (10, 1, N'Off-credit**', NULL, 1, CAST(N'2019-07-16T13:40:26.667' AS DateTime), CAST(N'2019-07-16T13:40:26.667' AS DateTime), 1, NULL)
INSERT [dbo].[TBL_Options] ([Id], [Type], [Value], [Parent], [AddedBy], [AddedOn], [UpdatedOn], [UpdatedBy], [FaIcon]) VALUES (11, 3, N'Agriculture', NULL, 1, CAST(N'2019-07-16T13:43:20.747' AS DateTime), CAST(N'2019-07-16T13:43:20.747' AS DateTime), 1, NULL)
INSERT [dbo].[TBL_Options] ([Id], [Type], [Value], [Parent], [AddedBy], [AddedOn], [UpdatedOn], [UpdatedBy], [FaIcon]) VALUES (12, 3, N'Auto & Auto Components', NULL, 1, CAST(N'2019-07-16T13:52:15.933' AS DateTime), CAST(N'2019-07-16T13:52:15.933' AS DateTime), 1, NULL)
INSERT [dbo].[TBL_Options] ([Id], [Type], [Value], [Parent], [AddedBy], [AddedOn], [UpdatedOn], [UpdatedBy], [FaIcon]) VALUES (13, 3, N'Cement Plant', NULL, 1, CAST(N'2019-07-16T13:43:21.000' AS DateTime), CAST(N'2019-07-16T13:43:21.000' AS DateTime), 1, NULL)
INSERT [dbo].[TBL_Options] ([Id], [Type], [Value], [Parent], [AddedBy], [AddedOn], [UpdatedOn], [UpdatedBy], [FaIcon]) VALUES (14, 3, N'Construction/Infrastructure', NULL, 1, CAST(N'2019-07-16T13:43:21.000' AS DateTime), CAST(N'2019-07-16T13:43:21.000' AS DateTime), 1, NULL)
INSERT [dbo].[TBL_Options] ([Id], [Type], [Value], [Parent], [AddedBy], [AddedOn], [UpdatedOn], [UpdatedBy], [FaIcon]) VALUES (15, 3, N'Defence', NULL, 1, CAST(N'2019-07-16T13:43:21.000' AS DateTime), CAST(N'2019-07-16T13:43:21.000' AS DateTime), 1, NULL)
INSERT [dbo].[TBL_Options] ([Id], [Type], [Value], [Parent], [AddedBy], [AddedOn], [UpdatedOn], [UpdatedBy], [FaIcon]) VALUES (16, 3, N'Education/Training', NULL, 1, CAST(N'2019-07-16T13:43:21.000' AS DateTime), CAST(N'2019-07-16T13:43:21.000' AS DateTime), 1, NULL)
INSERT [dbo].[TBL_Options] ([Id], [Type], [Value], [Parent], [AddedBy], [AddedOn], [UpdatedOn], [UpdatedBy], [FaIcon]) VALUES (17, 3, N'Engineering', NULL, 1, CAST(N'2019-07-16T13:43:21.000' AS DateTime), CAST(N'2019-07-16T13:43:21.000' AS DateTime), 1, NULL)
INSERT [dbo].[TBL_Options] ([Id], [Type], [Value], [Parent], [AddedBy], [AddedOn], [UpdatedOn], [UpdatedBy], [FaIcon]) VALUES (18, 3, N'Fish Processing Plant', NULL, 1, CAST(N'2019-07-16T13:43:21.000' AS DateTime), CAST(N'2019-07-16T13:43:21.000' AS DateTime), 1, NULL)
INSERT [dbo].[TBL_Options] ([Id], [Type], [Value], [Parent], [AddedBy], [AddedOn], [UpdatedOn], [UpdatedBy], [FaIcon]) VALUES (19, 3, N'Healthcare', NULL, 1, CAST(N'2019-07-16T13:43:21.000' AS DateTime), CAST(N'2019-07-16T13:43:21.000' AS DateTime), 1, NULL)
INSERT [dbo].[TBL_Options] ([Id], [Type], [Value], [Parent], [AddedBy], [AddedOn], [UpdatedOn], [UpdatedBy], [FaIcon]) VALUES (20, 3, N'Housing', NULL, 1, CAST(N'2019-07-16T13:43:21.000' AS DateTime), CAST(N'2019-07-16T13:43:21.000' AS DateTime), 1, NULL)
INSERT [dbo].[TBL_Options] ([Id], [Type], [Value], [Parent], [AddedBy], [AddedOn], [UpdatedOn], [UpdatedBy], [FaIcon]) VALUES (21, 3, N'Irrigation', NULL, 1, CAST(N'2019-07-16T13:43:21.000' AS DateTime), CAST(N'2019-07-16T13:43:21.000' AS DateTime), 1, NULL)
INSERT [dbo].[TBL_Options] ([Id], [Type], [Value], [Parent], [AddedBy], [AddedOn], [UpdatedOn], [UpdatedBy], [FaIcon]) VALUES (22, 3, N'IT', NULL, 1, CAST(N'2019-07-16T13:43:21.000' AS DateTime), CAST(N'2019-07-16T13:43:21.000' AS DateTime), 1, NULL)
INSERT [dbo].[TBL_Options] ([Id], [Type], [Value], [Parent], [AddedBy], [AddedOn], [UpdatedOn], [UpdatedBy], [FaIcon]) VALUES (23, 3, N'Oil processing Equipments', NULL, 1, CAST(N'2019-07-16T13:43:21.000' AS DateTime), CAST(N'2019-07-16T13:43:21.000' AS DateTime), 1, NULL)
INSERT [dbo].[TBL_Options] ([Id], [Type], [Value], [Parent], [AddedBy], [AddedOn], [UpdatedOn], [UpdatedBy], [FaIcon]) VALUES (24, 3, N'Oil Refinery', NULL, 1, CAST(N'2019-07-16T13:43:21.000' AS DateTime), CAST(N'2019-07-16T13:43:21.000' AS DateTime), 1, NULL)
INSERT [dbo].[TBL_Options] ([Id], [Type], [Value], [Parent], [AddedBy], [AddedOn], [UpdatedOn], [UpdatedBy], [FaIcon]) VALUES (25, 3, N'Others', NULL, 1, CAST(N'2019-07-16T13:43:21.000' AS DateTime), CAST(N'2019-07-16T13:43:21.000' AS DateTime), 1, NULL)
INSERT [dbo].[TBL_Options] ([Id], [Type], [Value], [Parent], [AddedBy], [AddedOn], [UpdatedOn], [UpdatedBy], [FaIcon]) VALUES (26, 3, N'Ports', NULL, 1, CAST(N'2019-07-16T13:43:21.000' AS DateTime), CAST(N'2019-07-16T13:43:21.000' AS DateTime), 1, NULL)
INSERT [dbo].[TBL_Options] ([Id], [Type], [Value], [Parent], [AddedBy], [AddedOn], [UpdatedOn], [UpdatedBy], [FaIcon]) VALUES (27, 3, N'Power', NULL, 1, CAST(N'2019-07-16T13:43:21.000' AS DateTime), CAST(N'2019-07-16T13:43:21.000' AS DateTime), 1, NULL)
INSERT [dbo].[TBL_Options] ([Id], [Type], [Value], [Parent], [AddedBy], [AddedOn], [UpdatedOn], [UpdatedBy], [FaIcon]) VALUES (28, 3, N'Railways', NULL, 1, CAST(N'2019-07-16T13:43:21.000' AS DateTime), CAST(N'2019-07-16T13:43:21.000' AS DateTime), 1, NULL)
INSERT [dbo].[TBL_Options] ([Id], [Type], [Value], [Parent], [AddedBy], [AddedOn], [UpdatedOn], [UpdatedBy], [FaIcon]) VALUES (29, 3, N'Roads & Transport', NULL, 1, CAST(N'2019-07-16T13:43:21.000' AS DateTime), CAST(N'2019-07-16T13:43:21.000' AS DateTime), 1, NULL)
INSERT [dbo].[TBL_Options] ([Id], [Type], [Value], [Parent], [AddedBy], [AddedOn], [UpdatedOn], [UpdatedBy], [FaIcon]) VALUES (30, 3, N'Rural Electrification', NULL, 1, CAST(N'2019-07-16T13:43:21.000' AS DateTime), CAST(N'2019-07-16T13:43:21.000' AS DateTime), 1, NULL)
INSERT [dbo].[TBL_Options] ([Id], [Type], [Value], [Parent], [AddedBy], [AddedOn], [UpdatedOn], [UpdatedBy], [FaIcon]) VALUES (31, 3, N'Solar', NULL, 1, CAST(N'2019-07-16T13:43:21.000' AS DateTime), CAST(N'2019-07-16T13:43:21.000' AS DateTime), 1, NULL)
INSERT [dbo].[TBL_Options] ([Id], [Type], [Value], [Parent], [AddedBy], [AddedOn], [UpdatedOn], [UpdatedBy], [FaIcon]) VALUES (32, 3, N'Sugar Plant', NULL, 1, CAST(N'2019-07-16T13:43:21.000' AS DateTime), CAST(N'2019-07-16T13:43:21.000' AS DateTime), 1, NULL)
INSERT [dbo].[TBL_Options] ([Id], [Type], [Value], [Parent], [AddedBy], [AddedOn], [UpdatedOn], [UpdatedBy], [FaIcon]) VALUES (33, 3, N'Technology and communication', NULL, 1, CAST(N'2019-07-16T13:43:21.000' AS DateTime), CAST(N'2019-07-16T13:43:21.000' AS DateTime), 1, NULL)
INSERT [dbo].[TBL_Options] ([Id], [Type], [Value], [Parent], [AddedBy], [AddedOn], [UpdatedOn], [UpdatedBy], [FaIcon]) VALUES (34, 3, N'Textile Sector', NULL, 1, CAST(N'2019-07-16T13:43:21.000' AS DateTime), CAST(N'2019-07-16T13:43:21.000' AS DateTime), 1, NULL)
INSERT [dbo].[TBL_Options] ([Id], [Type], [Value], [Parent], [AddedBy], [AddedOn], [UpdatedOn], [UpdatedBy], [FaIcon]) VALUES (35, 3, N'Various', NULL, 1, CAST(N'2019-07-16T13:43:21.000' AS DateTime), CAST(N'2019-07-16T13:43:21.000' AS DateTime), 1, NULL)
INSERT [dbo].[TBL_Options] ([Id], [Type], [Value], [Parent], [AddedBy], [AddedOn], [UpdatedOn], [UpdatedBy], [FaIcon]) VALUES (36, 3, N'Water', NULL, 1, CAST(N'2019-07-16T13:43:21.000' AS DateTime), CAST(N'2019-07-16T13:43:21.000' AS DateTime), 1, NULL)
INSERT [dbo].[TBL_Options] ([Id], [Type], [Value], [Parent], [AddedBy], [AddedOn], [UpdatedOn], [UpdatedBy], [FaIcon]) VALUES (37, 5, N'CLL06-Libor', NULL, 1, CAST(N'2019-07-16T13:55:27.270' AS DateTime), CAST(N'2019-07-16T13:55:27.270' AS DateTime), 1, NULL)
INSERT [dbo].[TBL_Options] ([Id], [Type], [Value], [Parent], [AddedBy], [AddedOn], [UpdatedOn], [UpdatedBy], [FaIcon]) VALUES (38, 5, N'Interest Only', NULL, 1, CAST(N'2019-07-16T13:55:55.963' AS DateTime), CAST(N'2019-07-16T13:56:14.163' AS DateTime), 1, NULL)
INSERT [dbo].[TBL_Options] ([Id], [Type], [Value], [Parent], [AddedBy], [AddedOn], [UpdatedOn], [UpdatedBy], [FaIcon]) VALUES (39, 6, N'DEA', NULL, 1, CAST(N'2019-07-16T13:56:27.997' AS DateTime), CAST(N'2019-07-16T13:56:27.997' AS DateTime), 1, NULL)
INSERT [dbo].[TBL_Options] ([Id], [Type], [Value], [Parent], [AddedBy], [AddedOn], [UpdatedOn], [UpdatedBy], [FaIcon]) VALUES (40, 6, N'MEA', NULL, 1, CAST(N'2019-07-16T13:56:39.807' AS DateTime), CAST(N'2019-07-16T13:56:39.807' AS DateTime), 1, NULL)
INSERT [dbo].[TBL_Options] ([Id], [Type], [Value], [Parent], [AddedBy], [AddedOn], [UpdatedOn], [UpdatedBy], [FaIcon]) VALUES (41, 9, N'Company', NULL, 1, CAST(N'2019-07-16T13:57:06.137' AS DateTime), CAST(N'2019-07-16T13:57:06.137' AS DateTime), 1, NULL)
INSERT [dbo].[TBL_Options] ([Id], [Type], [Value], [Parent], [AddedBy], [AddedOn], [UpdatedOn], [UpdatedBy], [FaIcon]) VALUES (42, 9, N'Individual', NULL, 1, CAST(N'2019-07-16T13:57:18.923' AS DateTime), CAST(N'2019-07-16T13:57:18.923' AS DateTime), 1, NULL)
INSERT [dbo].[TBL_Options] ([Id], [Type], [Value], [Parent], [AddedBy], [AddedOn], [UpdatedOn], [UpdatedBy], [FaIcon]) VALUES (43, 7, N'CC', NULL, 1, CAST(N'2019-07-16T13:58:29.483' AS DateTime), CAST(N'2019-07-16T13:58:29.483' AS DateTime), 1, NULL)
INSERT [dbo].[TBL_Options] ([Id], [Type], [Value], [Parent], [AddedBy], [AddedOn], [UpdatedOn], [UpdatedBy], [FaIcon]) VALUES (44, 7, N'EPC-TK', NULL, 1, CAST(N'2019-07-16T13:58:40.397' AS DateTime), CAST(N'2019-07-16T13:58:40.397' AS DateTime), 1, NULL)
INSERT [dbo].[TBL_Options] ([Id], [Type], [Value], [Parent], [AddedBy], [AddedOn], [UpdatedOn], [UpdatedBy], [FaIcon]) VALUES (45, 7, N'PMC', NULL, 1, CAST(N'2019-07-16T13:58:58.467' AS DateTime), CAST(N'2019-07-16T13:58:58.467' AS DateTime), 1, NULL)
INSERT [dbo].[TBL_Options] ([Id], [Type], [Value], [Parent], [AddedBy], [AddedOn], [UpdatedOn], [UpdatedBy], [FaIcon]) VALUES (46, 7, N'SPPLY', NULL, 1, CAST(N'2019-07-16T13:59:10.753' AS DateTime), CAST(N'2019-07-16T13:59:10.753' AS DateTime), 1, NULL)
INSERT [dbo].[TBL_Options] ([Id], [Type], [Value], [Parent], [AddedBy], [AddedOn], [UpdatedOn], [UpdatedBy], [FaIcon]) VALUES (47, 2, N'MILD as per 2004 Guidelines', NULL, 1, CAST(N'2019-09-17T14:56:54.663' AS DateTime), CAST(N'2019-11-05T12:58:55.083' AS DateTime), 1, NULL)
INSERT [dbo].[TBL_Options] ([Id], [Type], [Value], [Parent], [AddedBy], [AddedOn], [UpdatedOn], [UpdatedBy], [FaIcon]) VALUES (48, 10, N'FATF', NULL, 1, CAST(N'2019-09-26T15:14:38.507' AS DateTime), CAST(N'2019-09-26T15:14:38.507' AS DateTime), 1, NULL)
INSERT [dbo].[TBL_Options] ([Id], [Type], [Value], [Parent], [AddedBy], [AddedOn], [UpdatedOn], [UpdatedBy], [FaIcon]) VALUES (49, 2, N'LIC-LDC as per 2010 Guidelines', NULL, 1, CAST(N'2019-11-05T12:57:06.593' AS DateTime), CAST(N'2019-11-05T13:01:21.027' AS DateTime), 1, NULL)
INSERT [dbo].[TBL_Options] ([Id], [Type], [Value], [Parent], [AddedBy], [AddedOn], [UpdatedOn], [UpdatedBy], [FaIcon]) VALUES (50, 2, N'HIPC as per 2010 Guidelines', NULL, 1, CAST(N'2019-11-05T12:59:03.773' AS DateTime), CAST(N'2019-11-05T13:01:29.530' AS DateTime), 1, NULL)
INSERT [dbo].[TBL_Options] ([Id], [Type], [Value], [Parent], [AddedBy], [AddedOn], [UpdatedOn], [UpdatedBy], [FaIcon]) VALUES (51, 2, N'LIHD as per 2004 Guidelines', NULL, 1, CAST(N'2019-11-05T12:59:47.977' AS DateTime), CAST(N'2019-11-05T13:01:41.880' AS DateTime), 1, NULL)
INSERT [dbo].[TBL_Options] ([Id], [Type], [Value], [Parent], [AddedBy], [AddedOn], [UpdatedOn], [UpdatedBy], [FaIcon]) VALUES (52, 2, N'MIC as per 2010 Guidelines', NULL, 1, CAST(N'2019-11-05T13:00:16.087' AS DateTime), CAST(N'2019-11-05T13:01:49.500' AS DateTime), 1, NULL)
INSERT [dbo].[TBL_Options] ([Id], [Type], [Value], [Parent], [AddedBy], [AddedOn], [UpdatedOn], [UpdatedBy], [FaIcon]) VALUES (53, 2, N'HIPC as per 2004 Guidelines', NULL, 1, CAST(N'2019-11-05T13:00:47.693' AS DateTime), CAST(N'2019-11-05T13:01:56.397' AS DateTime), 1, NULL)
INSERT [dbo].[TBL_Options] ([Id], [Type], [Value], [Parent], [AddedBy], [AddedOn], [UpdatedOn], [UpdatedBy], [FaIcon]) VALUES (54, 11, N'PreQualified', NULL, 1, CAST(N'2020-02-11T12:31:51.530' AS DateTime), CAST(N'2020-02-11T12:31:51.530' AS DateTime), 1, NULL)
INSERT [dbo].[TBL_Options] ([Id], [Type], [Value], [Parent], [AddedBy], [AddedOn], [UpdatedOn], [UpdatedBy], [FaIcon]) VALUES (55, 11, N'Pending', NULL, 1, CAST(N'2020-02-11T12:31:51.530' AS DateTime), CAST(N'2020-02-11T12:31:51.530' AS DateTime), 1, NULL)
INSERT [dbo].[TBL_Options] ([Id], [Type], [Value], [Parent], [AddedBy], [AddedOn], [UpdatedOn], [UpdatedBy], [FaIcon]) VALUES (56, 11, N'Disqualified', NULL, 1, CAST(N'2020-02-11T12:31:51.530' AS DateTime), CAST(N'2020-02-11T12:31:51.530' AS DateTime), 1, NULL)
INSERT [dbo].[TBL_Options] ([Id], [Type], [Value], [Parent], [AddedBy], [AddedOn], [UpdatedOn], [UpdatedBy], [FaIcon]) VALUES (57, 2, N'MIHD as per 2004 Guidelines', NULL, 1, CAST(N'2020-02-11T12:31:51.530' AS DateTime), CAST(N'2020-02-11T12:31:51.530' AS DateTime), 1, NULL)
INSERT [dbo].[TBL_Options] ([Id], [Type], [Value], [Parent], [AddedBy], [AddedOn], [UpdatedOn], [UpdatedBy], [FaIcon]) VALUES (58, 5, N'Fixed', NULL, 1, CAST(N'2020-02-11T12:31:51.530' AS DateTime), CAST(N'2020-02-11T12:31:51.530' AS DateTime), 1, NULL)
INSERT [dbo].[TBL_Options] ([Id], [Type], [Value], [Parent], [AddedBy], [AddedOn], [UpdatedOn], [UpdatedBy], [FaIcon]) VALUES (59, 12, N'PQ process completed - Contract yet to be awarded', NULL, 1, CAST(N'2020-03-13T16:02:27.410' AS DateTime), CAST(N'2020-03-18T13:10:04.227' AS DateTime), 1, NULL)
INSERT [dbo].[TBL_Options] ([Id], [Type], [Value], [Parent], [AddedBy], [AddedOn], [UpdatedOn], [UpdatedBy], [FaIcon]) VALUES (60, 12, N'PQ process completed - Contract awarded', NULL, 1, CAST(N'2020-03-13T16:02:43.537' AS DateTime), CAST(N'2020-03-18T13:13:04.883' AS DateTime), 1, NULL)
INSERT [dbo].[TBL_Options] ([Id], [Type], [Value], [Parent], [AddedBy], [AddedOn], [UpdatedOn], [UpdatedBy], [FaIcon]) VALUES (61, 12, N'PQ to be Reinvited/Cancelled', NULL, 1, CAST(N'2020-03-18T13:14:35.040' AS DateTime), CAST(N'2020-03-18T13:14:35.040' AS DateTime), 1, NULL)
INSERT [dbo].[TBL_Options] ([Id], [Type], [Value], [Parent], [AddedBy], [AddedOn], [UpdatedOn], [UpdatedBy], [FaIcon]) VALUES (62, 12, N'PQ document under preparation', NULL, 1, CAST(N'2020-03-18T13:15:56.203' AS DateTime), CAST(N'2020-03-18T13:15:56.203' AS DateTime), 1, NULL)
INSERT [dbo].[TBL_Options] ([Id], [Type], [Value], [Parent], [AddedBy], [AddedOn], [UpdatedOn], [UpdatedBy], [FaIcon]) VALUES (63, 12, N'PQ uploaded presently on website', NULL, 1, CAST(N'2020-03-18T13:17:18.477' AS DateTime), CAST(N'2020-03-18T13:17:18.477' AS DateTime), 1, NULL)
INSERT [dbo].[TBL_Options] ([Id], [Type], [Value], [Parent], [AddedBy], [AddedOn], [UpdatedOn], [UpdatedBy], [FaIcon]) VALUES (64, 12, N'PQ opened and under evaluation', NULL, 1, CAST(N'2020-03-18T13:18:50.027' AS DateTime), CAST(N'2020-03-18T13:18:50.027' AS DateTime), 1, NULL)
SET IDENTITY_INSERT [dbo].[TBL_Options] OFF

SET IDENTITY_INSERT [dbo].[TBL_Classifications] ON 
INSERT [dbo].[TBL_Classifications] ([Id], [Name]) VALUES (1, N'Cat I')
INSERT [dbo].[TBL_Classifications] ([Id], [Name]) VALUES (2, N'Cat II')
INSERT [dbo].[TBL_Classifications] ([Id], [Name]) VALUES (3, N'Cat III')
SET IDENTITY_INSERT [dbo].[TBL_Classifications] OFF

SET IDENTITY_INSERT [dbo].[TBL_EmailRules] ON 
INSERT [dbo].[TBL_EmailRules] ([Id], [OffMonth], [OffWeek], [OffDay], [FreqMonth], [FreqWeek], [FreqDay], [RuleFor], [RuleName], [IsActive]) VALUES (1, 6, 0, 0, 1, 0, 0, N'Contract', N'ContractRules.TerminalDateofDisbursement', 1)
INSERT [dbo].[TBL_EmailRules] ([Id], [OffMonth], [OffWeek], [OffDay], [FreqMonth], [FreqWeek], [FreqDay], [RuleFor], [RuleName], [IsActive]) VALUES (2, 6, 0, 0, 1, 0, 0, N'Contract', N'ContractRules.AdvancePaymentGuarantee', 1)
INSERT [dbo].[TBL_EmailRules] ([Id], [OffMonth], [OffWeek], [OffDay], [FreqMonth], [FreqWeek], [FreqDay], [RuleFor], [RuleName], [IsActive]) VALUES (3, 6, 0, 0, 1, 0, 0, N'Contract', N'ContractRules.PerformanceGuarantee', 1)
INSERT [dbo].[TBL_EmailRules] ([Id], [OffMonth], [OffWeek], [OffDay], [FreqMonth], [FreqWeek], [FreqDay], [RuleFor], [RuleName], [IsActive]) VALUES (4, 6, 0, 0, 1, 0, 0, N'Contract', N'ContractRules.ScheduleCompleteDate', 1)
INSERT [dbo].[TBL_EmailRules] ([Id], [OffMonth], [OffWeek], [OffDay], [FreqMonth], [FreqWeek], [FreqDay], [RuleFor], [RuleName], [IsActive]) VALUES (5, 6, 0, 0, 1, 0, 0, N'Contract', N'ContractRules.FirstCommitmentFeeCat1', 1)
INSERT [dbo].[TBL_EmailRules] ([Id], [OffMonth], [OffWeek], [OffDay], [FreqMonth], [FreqWeek], [FreqDay], [RuleFor], [RuleName], [IsActive]) VALUES (6, 6, 0, 0, 1, 0, 0, N'Contract', N'ContractRules.FirstCommitmentFeeOCat', 1)
INSERT [dbo].[TBL_EmailRules] ([Id], [OffMonth], [OffWeek], [OffDay], [FreqMonth], [FreqWeek], [FreqDay], [RuleFor], [RuleName], [IsActive]) VALUES (7, 0, 1, 0, 0, 0, 1, N'Project', N'ProjectRules.BidsSubmissionPQ', 1)
INSERT [dbo].[TBL_EmailRules] ([Id], [OffMonth], [OffWeek], [OffDay], [FreqMonth], [FreqWeek], [FreqDay], [RuleFor], [RuleName], [IsActive]) VALUES (8, 0, 0, 0, 1, 0, 0, N'LOC', N'LocRules.BalanceConfirmation', 1)
INSERT [dbo].[TBL_EmailRules] ([Id], [OffMonth], [OffWeek], [OffDay], [FreqMonth], [FreqWeek], [FreqDay], [RuleFor], [RuleName], [IsActive]) VALUES (9, 6, 0, 0, 0, 0, 15, N'LOC', N'LocRules.AgreementSigningExp', 1)
INSERT [dbo].[TBL_EmailRules] ([Id], [OffMonth], [OffWeek], [OffDay], [FreqMonth], [FreqWeek], [FreqDay], [RuleFor], [RuleName], [IsActive]) VALUES (10, 6, 0, 0, 1, 0, 0, N'LOC', N'LocRules.ContractsExpiry', 1)
INSERT [dbo].[TBL_EmailRules] ([Id], [OffMonth], [OffWeek], [OffDay], [FreqMonth], [FreqWeek], [FreqDay], [RuleFor], [RuleName], [IsActive]) VALUES (11, 0, 0, 0, 0, 1, 0, N'LOC', N'LocRules.InterestDemand', 1)
INSERT [dbo].[TBL_EmailRules] ([Id], [OffMonth], [OffWeek], [OffDay], [FreqMonth], [FreqWeek], [FreqDay], [RuleFor], [RuleName], [IsActive]) VALUES (12, 0, 0, 0, 0, 1, 0, N'LOC', N'LocRules.PrincipalDemand', 1)
INSERT [dbo].[TBL_EmailRules] ([Id], [OffMonth], [OffWeek], [OffDay], [FreqMonth], [FreqWeek], [FreqDay], [RuleFor], [RuleName], [IsActive]) VALUES (13, 1, 0, 0, 0, 1, 0, N'LOC', N'LocRules.CommitmentFee', 1)
SET IDENTITY_INSERT [dbo].[TBL_EmailRules] OFF

SET IDENTITY_INSERT [dbo].[TBL_Status] ON 
INSERT [dbo].[TBL_Status] ([Id], [Name]) VALUES (1, N'Under Prep/DPR Pending')
INSERT [dbo].[TBL_Status] ([Id], [Name]) VALUES (2, N'PQ in Progress')
INSERT [dbo].[TBL_Status] ([Id], [Name]) VALUES (3, N'Tendering In Progress')
INSERT [dbo].[TBL_Status] ([Id], [Name]) VALUES (4, N'Execution In Progress')
INSERT [dbo].[TBL_Status] ([Id], [Name]) VALUES (5, N'Completed')
SET IDENTITY_INSERT [dbo].[TBL_Status] OFF

SET IDENTITY_INSERT [dbo].[TBL_String_Mapper] ON 
INSERT [dbo].[TBL_String_Mapper] ([Id], [Key], [Value]) VALUES (1, N'DuplicateCountryError', N'Country already exists!')
INSERT [dbo].[TBL_String_Mapper] ([Id], [Key], [Value]) VALUES (2, N'DuplicateCIFError', N'CIF already exists!')
INSERT [dbo].[TBL_String_Mapper] ([Id], [Key], [Value]) VALUES (3, N'GeneralUpdateError', N'Something went wrong while updating the data. Please try again and then contact support.')
INSERT [dbo].[TBL_String_Mapper] ([Id], [Key], [Value]) VALUES (4, N'DeleteCountryError', N'Country cannot be deleted.')
INSERT [dbo].[TBL_String_Mapper] ([Id], [Key], [Value]) VALUES (5, N'DataNotFoundError', N'Data not Found!')
INSERT [dbo].[TBL_String_Mapper] ([Id], [Key], [Value]) VALUES (6, N'DuplicateOptionTypeError', N'Option value already exists!')
INSERT [dbo].[TBL_String_Mapper] ([Id], [Key], [Value]) VALUES (1003, N'AdvPmtGrntAmount', N'Advance Payment Amount')
INSERT [dbo].[TBL_String_Mapper] ([Id], [Key], [Value]) VALUES (1004, N'GeneralUpdateSuccess', N'Data saved successfully!')
INSERT [dbo].[TBL_String_Mapper] ([Id], [Key], [Value]) VALUES (1005, N'ContractRules.TerminalDateofDisbursement', N'TerminalDateofDisbursement.html')
INSERT [dbo].[TBL_String_Mapper] ([Id], [Key], [Value]) VALUES (1006, N'ContractRules.AdvancePaymentGuarantee', N'AdvancePaymentGuarantee.html')
INSERT [dbo].[TBL_String_Mapper] ([Id], [Key], [Value]) VALUES (1007, N'ContractRules.PerformanceGuarantee', N'PerformanceGuarantee.html')
INSERT [dbo].[TBL_String_Mapper] ([Id], [Key], [Value]) VALUES (1008, N'ContractRules.ScheduleCompleteDate', N'ScheduleCompleteDate.html')
INSERT [dbo].[TBL_String_Mapper] ([Id], [Key], [Value]) VALUES (1009, N'ContractRules.FirstCommitmentFeeCat1', N'FirstCommitmentFeeCat1.html')
INSERT [dbo].[TBL_String_Mapper] ([Id], [Key], [Value]) VALUES (1010, N'ContractRules.FirstCommitmentFeeOCat', N'FirstCommitmentFeeOCat.html')
INSERT [dbo].[TBL_String_Mapper] ([Id], [Key], [Value]) VALUES (1011, N'ProjectRules.BidsSubmissionPQ', N'BidsSubmissionPQ.html')
INSERT [dbo].[TBL_String_Mapper] ([Id], [Key], [Value]) VALUES (1012, N'LocRules.BalanceConfirmation', N'BalanceConfirmation.html')
INSERT [dbo].[TBL_String_Mapper] ([Id], [Key], [Value]) VALUES (1013, N'LocRules.AgreementSigningExp', N'AgreementSigningExp.html')
INSERT [dbo].[TBL_String_Mapper] ([Id], [Key], [Value]) VALUES (1014, N'LocRules.ContractsExpiry', N'ContractsExpiry.html')
INSERT [dbo].[TBL_String_Mapper] ([Id], [Key], [Value]) VALUES (1015, N'LocRules.InterestDemand', N'InterestDemand.html')
INSERT [dbo].[TBL_String_Mapper] ([Id], [Key], [Value]) VALUES (1016, N'LocRules.PrincipalDemand', N'PrincipalDemand.html')
INSERT [dbo].[TBL_String_Mapper] ([Id], [Key], [Value]) VALUES (1017, N'LocRules.CommitmentFee', N'CommitmentFee.html')
INSERT [dbo].[TBL_String_Mapper] ([Id], [Key], [Value]) VALUES (1018, N'EmailUserName', N'test@gmail.com')
INSERT [dbo].[TBL_String_Mapper] ([Id], [Key], [Value]) VALUES (1019, N'EmailPort', N'587')
INSERT [dbo].[TBL_String_Mapper] ([Id], [Key], [Value]) VALUES (1020, N'EmailSMTP', N'smtp.domain.com')
INSERT [dbo].[TBL_String_Mapper] ([Id], [Key], [Value]) VALUES (1021, N'EmailPassword', N'p@ssw0rd')
INSERT [dbo].[TBL_String_Mapper] ([Id], [Key], [Value]) VALUES (1022, N'DefaultEmail', N'test2@gmail.com')
INSERT [dbo].[TBL_String_Mapper] ([Id], [Key], [Value]) VALUES (1023, N'Subject.ContractRules.TerminalDateofDisbursement', N'Alert for Expiry of Terminal Disbursement Date of Contract')
INSERT [dbo].[TBL_String_Mapper] ([Id], [Key], [Value]) VALUES (1024, N'Subject.ContractRules.AdvancePaymentGuarantee', N'Alert for Expiry of Advance Payment Guarantee of Contract')
INSERT [dbo].[TBL_String_Mapper] ([Id], [Key], [Value]) VALUES (1025, N'Subject.ContractRules.PerformanceGuarantee', N'Alert for Expiry of Performance Guarantee of Contract')
INSERT [dbo].[TBL_String_Mapper] ([Id], [Key], [Value]) VALUES (1026, N'Subject.ContractRules.ScheduleCompleteDate', N'Alert for Scheduled completion date of Contract')
INSERT [dbo].[TBL_String_Mapper] ([Id], [Key], [Value]) VALUES (1027, N'Subject.ContractRules.FirstCommitmentFeeCat1', N'Alert for First Commitment Fee Cat1')
INSERT [dbo].[TBL_String_Mapper] ([Id], [Key], [Value]) VALUES (1028, N'Subject.ContractRules.FirstCommitmentFeeOCat', N'Alert for First Commitment Fee Cat0')
INSERT [dbo].[TBL_String_Mapper] ([Id], [Key], [Value]) VALUES (1029, N'Subject.ProjectRules.BidsSubmissionPQ', N'Alert for bid submission')
INSERT [dbo].[TBL_String_Mapper] ([Id], [Key], [Value]) VALUES (1030, N'Subject.LocRules.BalanceConfirmation', N'Alert for Balance Confirmation Availability')
INSERT [dbo].[TBL_String_Mapper] ([Id], [Key], [Value]) VALUES (1031, N'Subject.LocRules.AgreementSigningExp', N'Alert for Balance Agreement Expiring')
INSERT [dbo].[TBL_String_Mapper] ([Id], [Key], [Value]) VALUES (1032, N'Subject.LocRules.ContractsExpiry', N'Alert for Balance Contracts Expiring')
INSERT [dbo].[TBL_String_Mapper] ([Id], [Key], [Value]) VALUES (1033, N'Subject.LocRules.InterestDemand', N'Alert for Interest Demand')
INSERT [dbo].[TBL_String_Mapper] ([Id], [Key], [Value]) VALUES (1034, N'Subject.LocRules.PrincipalDemand', N'Alert for Principal Demand')
INSERT [dbo].[TBL_String_Mapper] ([Id], [Key], [Value]) VALUES (1035, N'Subject.LocRules.CommitmentFee', N'Alert for Commitment Fee')
INSERT [dbo].[TBL_String_Mapper] ([Id], [Key], [Value]) VALUES (1036, N'MdAppDate', N'DEA Date')
INSERT [dbo].[TBL_String_Mapper] ([Id], [Key], [Value]) VALUES (1037, N'GoogleAPIKey', N'YOUR_API_KEY')
SET IDENTITY_INSERT [dbo].[TBL_String_Mapper] OFF

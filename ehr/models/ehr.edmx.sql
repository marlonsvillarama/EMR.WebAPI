
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 09/30/2018 18:58:03
-- Generated from EDMX file: D:\DEV\PROJECTS\git\EMR.WebAPI\ehr\models\ehr.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [HK_Kogan];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_ClaimLineDateClaimLine]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ClaimLineDates] DROP CONSTRAINT [FK_ClaimLineDateClaimLine];
GO
IF OBJECT_ID(N'[dbo].[FK_ClaimLineRefClaimLine]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ClaimLineReferences] DROP CONSTRAINT [FK_ClaimLineRefClaimLine];
GO
IF OBJECT_ID(N'[dbo].[FK_ClaimLineDrugClaimLine]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ClaimLineDrugs] DROP CONSTRAINT [FK_ClaimLineDrugClaimLine];
GO
IF OBJECT_ID(N'[dbo].[FK_ClaimLineAttachmentClaimLine]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ClaimLineAttachments] DROP CONSTRAINT [FK_ClaimLineAttachmentClaimLine];
GO
IF OBJECT_ID(N'[dbo].[FK_ClaimLineDurableClaimLine]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ClaimLineDurables] DROP CONSTRAINT [FK_ClaimLineDurableClaimLine];
GO
IF OBJECT_ID(N'[dbo].[FK_ClaimLineContractClaimLine]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ClaimLineContracts] DROP CONSTRAINT [FK_ClaimLineContractClaimLine];
GO
IF OBJECT_ID(N'[dbo].[FK_ClaimLineDialysisClaimLine]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ClaimLineDialysis] DROP CONSTRAINT [FK_ClaimLineDialysisClaimLine];
GO
IF OBJECT_ID(N'[dbo].[FK_ClaimLinePricingClaimLine]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ClaimLinePricings] DROP CONSTRAINT [FK_ClaimLinePricingClaimLine];
GO
IF OBJECT_ID(N'[dbo].[FK_ClaimLineProductClaimLine]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ClaimLineProducts] DROP CONSTRAINT [FK_ClaimLineProductClaimLine];
GO
IF OBJECT_ID(N'[dbo].[FK_ClaimLineAmbulanceClaimLine]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ClaimLineAmbulances] DROP CONSTRAINT [FK_ClaimLineAmbulanceClaimLine];
GO
IF OBJECT_ID(N'[dbo].[FK_ClaimLineSupplementalClaimLine]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ClaimLineSupplementals] DROP CONSTRAINT [FK_ClaimLineSupplementalClaimLine];
GO
IF OBJECT_ID(N'[dbo].[FK_PlaceOfServiceClaim]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Claims] DROP CONSTRAINT [FK_PlaceOfServiceClaim];
GO
IF OBJECT_ID(N'[dbo].[FK_TaxonomyCodeProvider]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Providers] DROP CONSTRAINT [FK_TaxonomyCodeProvider];
GO
IF OBJECT_ID(N'[dbo].[FK_PayerSubscriber]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Subscribers] DROP CONSTRAINT [FK_PayerSubscriber];
GO
IF OBJECT_ID(N'[dbo].[FK_PayerSubscriber1]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Subscribers] DROP CONSTRAINT [FK_PayerSubscriber1];
GO
IF OBJECT_ID(N'[dbo].[FK_SubscriberClaim]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Claims] DROP CONSTRAINT [FK_SubscriberClaim];
GO
IF OBJECT_ID(N'[dbo].[FK_SubscriberClaim1]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Claims] DROP CONSTRAINT [FK_SubscriberClaim1];
GO
IF OBJECT_ID(N'[dbo].[FK_PatientClaim]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Claims] DROP CONSTRAINT [FK_PatientClaim];
GO
IF OBJECT_ID(N'[dbo].[FK_FacilityClaim]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Claims] DROP CONSTRAINT [FK_FacilityClaim];
GO
IF OBJECT_ID(N'[dbo].[FK_PayerClaim]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Claims] DROP CONSTRAINT [FK_PayerClaim];
GO
IF OBJECT_ID(N'[dbo].[FK_PayerClaim1]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Claims] DROP CONSTRAINT [FK_PayerClaim1];
GO
IF OBJECT_ID(N'[dbo].[FK_ClaimClaimLine]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ClaimLines] DROP CONSTRAINT [FK_ClaimClaimLine];
GO
IF OBJECT_ID(N'[dbo].[FK_ClaimDateClaim]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Claims] DROP CONSTRAINT [FK_ClaimDateClaim];
GO
IF OBJECT_ID(N'[dbo].[FK_AddressProvider_Address]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AddressProvider] DROP CONSTRAINT [FK_AddressProvider_Address];
GO
IF OBJECT_ID(N'[dbo].[FK_AddressProvider_Provider]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AddressProvider] DROP CONSTRAINT [FK_AddressProvider_Provider];
GO
IF OBJECT_ID(N'[dbo].[FK_AddressSubscriber_Address]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AddressSubscriber] DROP CONSTRAINT [FK_AddressSubscriber_Address];
GO
IF OBJECT_ID(N'[dbo].[FK_AddressSubscriber_Subscriber]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AddressSubscriber] DROP CONSTRAINT [FK_AddressSubscriber_Subscriber];
GO
IF OBJECT_ID(N'[dbo].[FK_ClaimLineClaimLineDMERC]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ClaimLineDMERCs] DROP CONSTRAINT [FK_ClaimLineClaimLineDMERC];
GO
IF OBJECT_ID(N'[dbo].[FK_ProviderClaim1]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Claims] DROP CONSTRAINT [FK_ProviderClaim1];
GO
IF OBJECT_ID(N'[dbo].[FK_ProviderClaim]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Claims] DROP CONSTRAINT [FK_ProviderClaim];
GO
IF OBJECT_ID(N'[dbo].[FK_ClaimAccidentClaim]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Claims] DROP CONSTRAINT [FK_ClaimAccidentClaim];
GO
IF OBJECT_ID(N'[dbo].[FK_ClaimSupplementalClaim]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Claims] DROP CONSTRAINT [FK_ClaimSupplementalClaim];
GO
IF OBJECT_ID(N'[dbo].[FK_ClaimContractClaim]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Claims] DROP CONSTRAINT [FK_ClaimContractClaim];
GO
IF OBJECT_ID(N'[dbo].[FK_ClaimReferenceClaim]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Claims] DROP CONSTRAINT [FK_ClaimReferenceClaim];
GO
IF OBJECT_ID(N'[dbo].[FK_ClaimAmbulanceClaim]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Claims] DROP CONSTRAINT [FK_ClaimAmbulanceClaim];
GO
IF OBJECT_ID(N'[dbo].[FK_ClaimSpinalClaim]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Claims] DROP CONSTRAINT [FK_ClaimSpinalClaim];
GO
IF OBJECT_ID(N'[dbo].[FK_ClaimConditionClaim]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Claims] DROP CONSTRAINT [FK_ClaimConditionClaim];
GO
IF OBJECT_ID(N'[dbo].[FK_ClaimRepricingClaim]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Claims] DROP CONSTRAINT [FK_ClaimRepricingClaim];
GO
IF OBJECT_ID(N'[dbo].[FK_ClaimAmountClaim]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Claims] DROP CONSTRAINT [FK_ClaimAmountClaim];
GO
IF OBJECT_ID(N'[dbo].[FK_PatientSubscriber_Patient]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PatientSubscriber] DROP CONSTRAINT [FK_PatientSubscriber_Patient];
GO
IF OBJECT_ID(N'[dbo].[FK_PatientSubscriber_Subscriber]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PatientSubscriber] DROP CONSTRAINT [FK_PatientSubscriber_Subscriber];
GO
IF OBJECT_ID(N'[dbo].[FK_ProviderProvider_Provider]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ProviderProvider] DROP CONSTRAINT [FK_ProviderProvider_Provider];
GO
IF OBJECT_ID(N'[dbo].[FK_ProviderProvider_Provider1]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ProviderProvider] DROP CONSTRAINT [FK_ProviderProvider_Provider1];
GO
IF OBJECT_ID(N'[dbo].[FK_ErrorCodeErrorMessage]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ErrorMessages] DROP CONSTRAINT [FK_ErrorCodeErrorMessage];
GO
IF OBJECT_ID(N'[dbo].[FK_ErrorCodePayment]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Payments] DROP CONSTRAINT [FK_ErrorCodePayment];
GO
IF OBJECT_ID(N'[dbo].[FK_PaymentClaim]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Claims] DROP CONSTRAINT [FK_PaymentClaim];
GO
IF OBJECT_ID(N'[dbo].[FK_PaymentPaymentLine]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PaymentLines] DROP CONSTRAINT [FK_PaymentPaymentLine];
GO
IF OBJECT_ID(N'[dbo].[FK_UserAccount_User]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserAccount] DROP CONSTRAINT [FK_UserAccount_User];
GO
IF OBJECT_ID(N'[dbo].[FK_UserAccount_Account]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserAccount] DROP CONSTRAINT [FK_UserAccount_Account];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Addresses]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Addresses];
GO
IF OBJECT_ID(N'[dbo].[ClaimLines]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ClaimLines];
GO
IF OBJECT_ID(N'[dbo].[Claims]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Claims];
GO
IF OBJECT_ID(N'[dbo].[CPTCodes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CPTCodes];
GO
IF OBJECT_ID(N'[dbo].[Facilities]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Facilities];
GO
IF OBJECT_ID(N'[dbo].[ICDCodes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ICDCodes];
GO
IF OBJECT_ID(N'[dbo].[Payers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Payers];
GO
IF OBJECT_ID(N'[dbo].[Modifiers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Modifiers];
GO
IF OBJECT_ID(N'[dbo].[Patients]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Patients];
GO
IF OBJECT_ID(N'[dbo].[Providers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Providers];
GO
IF OBJECT_ID(N'[dbo].[PlaceOfServices]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PlaceOfServices];
GO
IF OBJECT_ID(N'[dbo].[TaxonomyCodes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TaxonomyCodes];
GO
IF OBJECT_ID(N'[dbo].[Subscribers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Subscribers];
GO
IF OBJECT_ID(N'[dbo].[SystemNotes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SystemNotes];
GO
IF OBJECT_ID(N'[dbo].[Users]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Users];
GO
IF OBJECT_ID(N'[dbo].[ClaimDates]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ClaimDates];
GO
IF OBJECT_ID(N'[dbo].[ClaimLineDMERCs]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ClaimLineDMERCs];
GO
IF OBJECT_ID(N'[dbo].[ClaimLineDates]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ClaimLineDates];
GO
IF OBJECT_ID(N'[dbo].[ClaimLineReferences]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ClaimLineReferences];
GO
IF OBJECT_ID(N'[dbo].[ClaimLineSupplementals]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ClaimLineSupplementals];
GO
IF OBJECT_ID(N'[dbo].[ClaimLineDrugs]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ClaimLineDrugs];
GO
IF OBJECT_ID(N'[dbo].[ClaimLineAttachments]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ClaimLineAttachments];
GO
IF OBJECT_ID(N'[dbo].[ClaimLineDurables]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ClaimLineDurables];
GO
IF OBJECT_ID(N'[dbo].[ClaimLineAmbulances]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ClaimLineAmbulances];
GO
IF OBJECT_ID(N'[dbo].[ClaimLineContracts]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ClaimLineContracts];
GO
IF OBJECT_ID(N'[dbo].[ClaimLineDialysis]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ClaimLineDialysis];
GO
IF OBJECT_ID(N'[dbo].[ClaimLinePricings]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ClaimLinePricings];
GO
IF OBJECT_ID(N'[dbo].[ClaimLineProducts]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ClaimLineProducts];
GO
IF OBJECT_ID(N'[dbo].[Submitters]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Submitters];
GO
IF OBJECT_ID(N'[dbo].[Receivers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Receivers];
GO
IF OBJECT_ID(N'[dbo].[Batches]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Batches];
GO
IF OBJECT_ID(N'[dbo].[PayToes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PayToes];
GO
IF OBJECT_ID(N'[dbo].[ClaimAccidents]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ClaimAccidents];
GO
IF OBJECT_ID(N'[dbo].[ClaimSupplementals]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ClaimSupplementals];
GO
IF OBJECT_ID(N'[dbo].[ClaimContracts]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ClaimContracts];
GO
IF OBJECT_ID(N'[dbo].[ClaimReferences]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ClaimReferences];
GO
IF OBJECT_ID(N'[dbo].[ClaimAmbulances]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ClaimAmbulances];
GO
IF OBJECT_ID(N'[dbo].[ClaimSpinals]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ClaimSpinals];
GO
IF OBJECT_ID(N'[dbo].[ClaimConditions]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ClaimConditions];
GO
IF OBJECT_ID(N'[dbo].[ClaimRepricings]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ClaimRepricings];
GO
IF OBJECT_ID(N'[dbo].[ClaimAmounts]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ClaimAmounts];
GO
IF OBJECT_ID(N'[dbo].[Insurers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Insurers];
GO
IF OBJECT_ID(N'[dbo].[UserRoles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserRoles];
GO
IF OBJECT_ID(N'[dbo].[Accounts]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Accounts];
GO
IF OBJECT_ID(N'[dbo].[Payments]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Payments];
GO
IF OBJECT_ID(N'[dbo].[PaymentLines]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PaymentLines];
GO
IF OBJECT_ID(N'[dbo].[ErrorCodes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ErrorCodes];
GO
IF OBJECT_ID(N'[dbo].[ErrorMessages]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ErrorMessages];
GO
IF OBJECT_ID(N'[dbo].[UserPreferences]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserPreferences];
GO
IF OBJECT_ID(N'[dbo].[AddressProvider]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AddressProvider];
GO
IF OBJECT_ID(N'[dbo].[AddressSubscriber]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AddressSubscriber];
GO
IF OBJECT_ID(N'[dbo].[PatientSubscriber]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PatientSubscriber];
GO
IF OBJECT_ID(N'[dbo].[ProviderProvider]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ProviderProvider];
GO
IF OBJECT_ID(N'[dbo].[UserAccount]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserAccount];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Addresses'
CREATE TABLE [dbo].[Addresses] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Address_1] nvarchar(max)  NULL,
    [Address_2] nvarchar(max)  NULL,
    [City] nvarchar(max)  NULL,
    [State] nvarchar(max)  NULL,
    [Zip] nvarchar(max)  NULL,
    [SystemNoteKey] nvarchar(max)  NULL
);
GO

-- Creating table 'ClaimLines'
CREATE TABLE [dbo].[ClaimLines] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Code] nvarchar(max)  NULL,
    [Modifier] nvarchar(max)  NULL,
    [StartDate] datetime  NOT NULL,
    [EndDate] datetime  NOT NULL,
    [Unit] nvarchar(max)  NULL,
    [OrderLine] int  NOT NULL,
    [SystemNoteKey] nvarchar(max)  NULL,
    [CPT] nvarchar(max)  NULL,
    [Amount] decimal(18,2)  NULL,
    [Pointer] nvarchar(max)  NULL,
    [Quantity] decimal(18,0)  NULL,
    [IsEmergency] bit  NULL,
    [EPSDT] bit  NULL,
    [FamilyPlanning] bit  NULL,
    [CopayExempt] bit  NULL,
    [DocumentType] nvarchar(max)  NULL,
    [ClaimId] int  NULL
);
GO

-- Creating table 'Claims'
CREATE TABLE [dbo].[Claims] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [AcceptAssignment] nvarchar(max)  NULL,
    [AmountBalance] decimal(18,2)  NULL,
    [AmountCopay] decimal(18,2)  NULL,
    [AmountTotal] decimal(18,2)  NULL,
    [FilingStatus] nvarchar(max)  NULL,
    [PrimaryPayerMemberID] nvarchar(max)  NULL,
    [SecondaryPayerMemberID] nvarchar(max)  NULL,
    [DiagnosisCodes] nvarchar(max)  NULL,
    [Relationship] nvarchar(max)  NULL,
    [PlaceOfServiceId] int  NULL,
    [PrimarySubscriberId] int  NULL,
    [SecondarySubscriberId] int  NULL,
    [PatientId] int  NULL,
    [FacilityId] int  NULL,
    [PrimaryPayerId] int  NULL,
    [SecondaryPayerId] int  NULL,
    [EmploymentRelated] bit  NULL,
    [OutsideLab] bit  NULL,
    [OutsideLabCharges] decimal(18,2)  NULL,
    [Identifier] nvarchar(max)  NULL,
    [DateOfService] datetime  NULL,
    [DateCreated] datetime  NULL,
    [ProviderId] int  NULL,
    [RenderingProviderId] int  NULL,
    [BillingProviderId] int  NULL,
    [AssignBenefits] nvarchar(max)  NULL,
    [AllowRelease] nvarchar(max)  NULL,
    [HasSignature] bit  NULL,
    [SpecialProgram] nvarchar(max)  NULL,
    [DelayReason] nvarchar(max)  NULL,
    [NoteType] nvarchar(max)  NULL,
    [Notes] nvarchar(max)  NULL,
    [HomeBound] bit  NULL,
    [SystemNoteKey] nvarchar(max)  NULL,
    [ClaimPayerType] nvarchar(max)  NULL,
    [DateModified] datetime  NULL,
    [Dates_Id] int  NULL,
    [Accident_Id] int  NULL,
    [Supplemental_Id] int  NULL,
    [Contract_Id] int  NULL,
    [Reference_Id] int  NULL,
    [Ambulance_Id] int  NULL,
    [Spinal_Id] int  NULL,
    [Condition_Id] int  NULL,
    [Repricing_Id] int  NULL,
    [Amounts_Id] int  NULL,
    [Payment_Id] int  NULL
);
GO

-- Creating table 'CPTCodes'
CREATE TABLE [dbo].[CPTCodes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Code] nvarchar(max)  NOT NULL,
    [Description] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Facilities'
CREATE TABLE [dbo].[Facilities] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Phone] nvarchar(max)  NULL,
    [Email] nvarchar(max)  NULL,
    [Fax] nvarchar(max)  NULL,
    [SystemNoteKey] nvarchar(max)  NULL,
    [Extension] nvarchar(max)  NULL,
    [NPI] nvarchar(max)  NULL,
    [TaxId] nvarchar(max)  NULL,
    [Address_1] nvarchar(max)  NULL,
    [Address_2] nvarchar(max)  NULL,
    [City] nvarchar(max)  NULL,
    [State] nvarchar(max)  NULL,
    [Zip] nvarchar(max)  NULL,
    [PaytoAddress_1] nvarchar(max)  NULL,
    [PaytoAddress_2] nvarchar(max)  NULL,
    [PaytoCity] nvarchar(max)  NULL,
    [PaytoState] nvarchar(max)  NULL,
    [PaytoZip] nvarchar(max)  NULL,
    [ContactName] nvarchar(max)  NULL
);
GO

-- Creating table 'ICDCodes'
CREATE TABLE [dbo].[ICDCodes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Code] nvarchar(max)  NOT NULL,
    [Description] nvarchar(max)  NULL,
    [ShortDescription] nvarchar(max)  NULL
);
GO

-- Creating table 'Payers'
CREATE TABLE [dbo].[Payers] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Type] nvarchar(max)  NULL,
    [Phone_1] nvarchar(max)  NULL,
    [Phone_2] nvarchar(max)  NULL,
    [Email] nvarchar(max)  NULL,
    [Fax] nvarchar(max)  NULL,
    [PayerId] nvarchar(max)  NOT NULL,
    [Notes] nvarchar(max)  NULL,
    [SystemNoteKey] nvarchar(max)  NULL,
    [Address_1] nvarchar(max)  NULL,
    [Address_2] nvarchar(max)  NULL,
    [City] nvarchar(max)  NULL,
    [State] nvarchar(max)  NULL,
    [Zip] nvarchar(max)  NULL
);
GO

-- Creating table 'Modifiers'
CREATE TABLE [dbo].[Modifiers] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Code] nvarchar(max)  NOT NULL,
    [Description] nvarchar(max)  NULL
);
GO

-- Creating table 'Patients'
CREATE TABLE [dbo].[Patients] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [AccountNumber] nvarchar(max)  NULL,
    [SystemNoteKey] nvarchar(max)  NULL,
    [IsDeceased] bit  NULL,
    [DateOfDeath] datetime  NULL,
    [Weight] decimal(18,2)  NULL,
    [IsPregnant] bit  NULL,
    [FirstName] nvarchar(max)  NULL,
    [MiddleName] nvarchar(max)  NULL,
    [LastName] nvarchar(max)  NULL,
    [DateOfBirth] datetime  NULL,
    [Gender] nvarchar(max)  NULL,
    [Phone] nvarchar(max)  NULL,
    [Email] nvarchar(max)  NULL,
    [Notes] nvarchar(max)  NULL,
    [Suffix] nvarchar(max)  NULL,
    [Address_1] nvarchar(max)  NULL,
    [Address_2] nvarchar(max)  NULL,
    [City] nvarchar(max)  NULL,
    [State] nvarchar(max)  NULL,
    [Zip] nvarchar(max)  NULL,
    [Relationship] nvarchar(max)  NULL,
    [DateModified] datetime  NULL
);
GO

-- Creating table 'Providers'
CREATE TABLE [dbo].[Providers] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [EIN] nvarchar(max)  NULL,
    [SSN] nvarchar(max)  NULL,
    [NPI] nvarchar(max)  NULL,
    [License] nvarchar(max)  NULL,
    [UPIN] nvarchar(max)  NULL,
    [SystemNoteKey] nvarchar(max)  NULL,
    [TaxonomyCodeId] int  NULL,
    [FirstName] nvarchar(max)  NULL,
    [MiddleName] nvarchar(max)  NULL,
    [LastName] nvarchar(max)  NULL,
    [Suffix] nvarchar(max)  NULL,
    [Phone_1] nvarchar(max)  NULL,
    [Email] nvarchar(max)  NULL,
    [Fax] nvarchar(max)  NULL,
    [Phone_2] nvarchar(max)  NULL,
    [Address_1] nvarchar(max)  NULL,
    [Address_2] nvarchar(max)  NULL,
    [City] nvarchar(max)  NULL,
    [State] nvarchar(max)  NULL,
    [Zip] nvarchar(max)  NULL,
    [IsCompany] bit  NOT NULL,
    [Credential] nvarchar(max)  NULL,
    [DateModified] datetime  NULL
);
GO

-- Creating table 'PlaceOfServices'
CREATE TABLE [dbo].[PlaceOfServices] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Code] nvarchar(max)  NOT NULL,
    [Description] nvarchar(max)  NULL,
    [SystemNoteKey] nvarchar(max)  NULL
);
GO

-- Creating table 'TaxonomyCodes'
CREATE TABLE [dbo].[TaxonomyCodes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Code] nvarchar(max)  NULL,
    [Grouping] nvarchar(max)  NULL,
    [Classification] nvarchar(max)  NULL,
    [Specialization] nvarchar(max)  NULL,
    [Definition] nvarchar(max)  NULL,
    [Notes] nvarchar(max)  NULL
);
GO

-- Creating table 'Subscribers'
CREATE TABLE [dbo].[Subscribers] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [SystemNoteKey] nvarchar(max)  NULL,
    [PrimaryMemberID] nvarchar(max)  NULL,
    [SecondaryMemberID] nvarchar(max)  NULL,
    [PrimaryExpiryDate] nvarchar(max)  NULL,
    [SecondaryExpiryDate] nvarchar(max)  NULL,
    [PrimaryPayerId] int  NULL,
    [SecondaryPayerId] int  NULL,
    [FirstName] nvarchar(max)  NULL,
    [MiddleName] nvarchar(max)  NULL,
    [LastName] nvarchar(max)  NULL,
    [DateOfBirth] datetime  NULL,
    [Gender] nvarchar(max)  NULL,
    [Phone_1] nvarchar(max)  NULL,
    [Phone_2] nvarchar(max)  NULL,
    [Email] nvarchar(max)  NULL,
    [Fax] nvarchar(max)  NULL,
    [Notes] nvarchar(max)  NULL,
    [Suffix] nvarchar(max)  NULL,
    [Address_1] nvarchar(max)  NULL,
    [Address_2] nvarchar(max)  NULL,
    [City] nvarchar(max)  NULL,
    [State] nvarchar(max)  NULL,
    [Zip] nvarchar(max)  NULL,
    [GroupName] nvarchar(max)  NULL,
    [GroupNumber] nvarchar(max)  NULL,
    [PrimaryPayerType] nvarchar(max)  NULL,
    [SecondaryPayerType] nvarchar(max)  NULL,
    [IsDeceased] bit  NULL,
    [DateOfDeath] datetime  NULL,
    [Weight] decimal(18,2)  NULL,
    [IsPregnant] bit  NULL,
    [SSN] nvarchar(max)  NULL,
    [PrimaryInsurerId] int  NULL,
    [SecondaryInsurerId] int  NULL,
    [DateModified] datetime  NULL
);
GO

-- Creating table 'SystemNotes'
CREATE TABLE [dbo].[SystemNotes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Key] nvarchar(max)  NOT NULL,
    [DateTime] datetime  NOT NULL,
    [Title] nvarchar(max)  NOT NULL,
    [Message] nvarchar(max)  NOT NULL,
    [UserId] int  NOT NULL,
    [OldValue] nvarchar(max)  NULL,
    [NewValue] nvarchar(max)  NULL
);
GO

-- Creating table 'Users'
CREATE TABLE [dbo].[Users] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [FirstName] nvarchar(max)  NOT NULL,
    [LastName] nvarchar(max)  NOT NULL,
    [UserName] nvarchar(max)  NULL,
    [Password] nvarchar(max)  NULL,
    [Email] nvarchar(max)  NULL,
    [IsInactive] bit  NOT NULL,
    [SystemNoteKey] nvarchar(max)  NULL,
    [Salt] nvarchar(max)  NULL,
    [DefaultAccount] int  NULL
);
GO

-- Creating table 'ClaimDates'
CREATE TABLE [dbo].[ClaimDates] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [OnsetOfCurrent] datetime  NULL,
    [InitialTreatment] datetime  NULL,
    [LastSeen] datetime  NULL,
    [Acute] datetime  NULL,
    [LastMenstrual] datetime  NULL,
    [LastXRay] datetime  NULL,
    [HearingVision] datetime  NULL,
    [LastWorked] datetime  NULL,
    [ReturnToWork] datetime  NULL,
    [Admission] datetime  NULL,
    [Discharge] datetime  NULL,
    [Assumed] datetime  NULL,
    [Property] datetime  NULL,
    [Repricer] datetime  NULL,
    [SystemNoteKey] nvarchar(max)  NULL,
    [Other] datetime  NULL,
    [DisabilityStart] datetime  NULL,
    [DisabilityEnd] datetime  NULL
);
GO

-- Creating table 'ClaimLineDMERCs'
CREATE TABLE [dbo].[ClaimLineDMERCs] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [DMERC_Response] nvarchar(max)  NULL,
    [DMERC_Code] nvarchar(max)  NULL,
    [DMERC_Code2] nvarchar(max)  NULL,
    [SystemNoteKey] nvarchar(max)  NULL,
    [ClaimLineClaimLineDMERC_ClaimLineDMERC_Id] int  NOT NULL
);
GO

-- Creating table 'ClaimLineDates'
CREATE TABLE [dbo].[ClaimLineDates] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Prescription] datetime  NULL,
    [Certification] datetime  NULL,
    [BeginTherapy] datetime  NULL,
    [LastCertification] datetime  NULL,
    [LastSeen] datetime  NULL,
    [Test] datetime  NULL,
    [Shipped] datetime  NULL,
    [LastXRay] datetime  NULL,
    [InitialTreatment] datetime  NULL,
    [SystemNoteKey] nvarchar(max)  NULL,
    [ServiceFrom] datetime  NULL,
    [ServiceTo] datetime  NULL,
    [ClaimLine_Id] int  NOT NULL
);
GO

-- Creating table 'ClaimLineReferences'
CREATE TABLE [dbo].[ClaimLineReferences] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Repriced] nvarchar(max)  NULL,
    [AdjustedRepriced] nvarchar(max)  NULL,
    [Prior] nvarchar(max)  NULL,
    [PriorIdentifier] nvarchar(max)  NULL,
    [PriorOtherPayer] nvarchar(max)  NULL,
    [ControlNumber] nvarchar(max)  NULL,
    [Mammography] nvarchar(max)  NULL,
    [CLIA] nvarchar(max)  NULL,
    [CLIAFacility] nvarchar(max)  NULL,
    [Immunization] nvarchar(max)  NULL,
    [Referral] nvarchar(max)  NULL,
    [ReferralIdentifier] nvarchar(max)  NULL,
    [ReferralOtherPayer] nvarchar(max)  NULL,
    [SystemNoteKey] nvarchar(max)  NULL,
    [ClaimLine_Id] int  NOT NULL
);
GO

-- Creating table 'ClaimLineSupplementals'
CREATE TABLE [dbo].[ClaimLineSupplementals] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [SalesTaxAmount] decimal(18,2)  NULL,
    [PostageAmount] decimal(18,2)  NULL,
    [FileInfo] nvarchar(max)  NULL,
    [NoteCode] nvarchar(max)  NULL,
    [NoteDescription] nvarchar(max)  NULL,
    [ThirdPartyCode] nvarchar(max)  NULL,
    [ThirdPartyDescription] nvarchar(max)  NULL,
    [PurchaseCode] nvarchar(max)  NULL,
    [PurchaseAmount] decimal(18,0)  NULL,
    [RejectCode] nvarchar(max)  NULL,
    [ComplianceCode] nvarchar(max)  NULL,
    [ExceptionCode] nvarchar(max)  NULL,
    [HospiceCode] nvarchar(max)  NULL,
    [ObsAnesUnits] int  NULL,
    [SystemNoteKey] nvarchar(max)  NULL,
    [ClaimLine_Id] int  NOT NULL
);
GO

-- Creating table 'ClaimLineDrugs'
CREATE TABLE [dbo].[ClaimLineDrugs] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Qualifier] nvarchar(max)  NULL,
    [Code] nvarchar(max)  NULL,
    [Quantity] int  NULL,
    [Unit] nvarchar(max)  NULL,
    [PrescriptionCode] nvarchar(max)  NULL,
    [PrescriptionNumber] nvarchar(max)  NULL,
    [SystemNoteKey] nvarchar(max)  NULL,
    [ClaimLine_Id] int  NOT NULL
);
GO

-- Creating table 'ClaimLineAttachments'
CREATE TABLE [dbo].[ClaimLineAttachments] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Type] nvarchar(max)  NULL,
    [Transmission] nvarchar(max)  NULL,
    [Code] nvarchar(max)  NULL,
    [SystemNoteKey] nvarchar(max)  NULL,
    [ClaimLine_Id] int  NOT NULL
);
GO

-- Creating table 'ClaimLineDurables'
CREATE TABLE [dbo].[ClaimLineDurables] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Transmission] nvarchar(max)  NULL,
    [Procedure] nvarchar(max)  NULL,
    [Quantity] nvarchar(max)  NULL,
    [Rental] decimal(18,0)  NULL,
    [Purchase] decimal(18,0)  NULL,
    [Frequency] nvarchar(max)  NULL,
    [CertCode] nvarchar(max)  NULL,
    [CertQuantity] decimal(18,0)  NULL,
    [SystemNoteKey] nvarchar(max)  NULL,
    [ClaimLine_Id] int  NOT NULL
);
GO

-- Creating table 'ClaimLineAmbulances'
CREATE TABLE [dbo].[ClaimLineAmbulances] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Weight] decimal(18,0)  NULL,
    [Reason] nvarchar(max)  NULL,
    [Quantity] decimal(18,0)  NULL,
    [RoundTrip] nvarchar(max)  NULL,
    [Stretcher] nvarchar(max)  NULL,
    [CertCode_1] nvarchar(max)  NULL,
    [CertCode_2] nvarchar(max)  NULL,
    [CertCode_3] nvarchar(max)  NULL,
    [CertCode_4] nvarchar(max)  NULL,
    [CertCode_5] nvarchar(max)  NULL,
    [CertResponse] nvarchar(max)  NULL,
    [PatientCount] int  NULL,
    [SystemNoteKey] nvarchar(max)  NULL,
    [ClaimLine_Id] int  NOT NULL
);
GO

-- Creating table 'ClaimLineContracts'
CREATE TABLE [dbo].[ClaimLineContracts] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Type] nvarchar(max)  NULL,
    [Amount] decimal(18,2)  NULL,
    [Percent] decimal(18,0)  NULL,
    [Code] nvarchar(max)  NULL,
    [Discount] decimal(18,0)  NULL,
    [Version] nvarchar(max)  NOT NULL,
    [SystemNoteKey] nvarchar(max)  NULL,
    [ClaimLine_Id] int  NOT NULL
);
GO

-- Creating table 'ClaimLineDialysis'
CREATE TABLE [dbo].[ClaimLineDialysis] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Code] nvarchar(max)  NULL,
    [Qualifier] nvarchar(max)  NULL,
    [Value] nvarchar(max)  NULL,
    [SystemNoteKey] nvarchar(max)  NULL,
    [ClaimLine_Id] int  NOT NULL
);
GO

-- Creating table 'ClaimLinePricings'
CREATE TABLE [dbo].[ClaimLinePricings] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Method] nvarchar(max)  NULL,
    [AmountAllowed] decimal(18,2)  NULL,
    [AmountSavings] decimal(18,2)  NULL,
    [OrgID] nvarchar(max)  NULL,
    [Rate] decimal(18,2)  NULL,
    [AmbGroup] nvarchar(max)  NULL,
    [AmbGroupAmount] decimal(18,2)  NULL,
    [ProductQualifier] nvarchar(max)  NULL,
    [HCPCS] nvarchar(max)  NULL,
    [Unit] nvarchar(max)  NULL,
    [Quantity] decimal(18,0)  NULL,
    [ComplianceCode] nvarchar(max)  NULL,
    [ExceptionCode] nvarchar(max)  NULL,
    [RejectReason] nvarchar(max)  NULL,
    [SystemNoteKey] nvarchar(max)  NULL,
    [ClaimLine_Id] int  NOT NULL
);
GO

-- Creating table 'ClaimLineProducts'
CREATE TABLE [dbo].[ClaimLineProducts] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Qualifier] nvarchar(max)  NULL,
    [Code] nvarchar(max)  NULL,
    [Unit] nvarchar(max)  NULL,
    [Quantity] decimal(18,0)  NULL,
    [SystemNoteKey] nvarchar(max)  NULL,
    [ClaimLine_Id] int  NOT NULL
);
GO

-- Creating table 'Submitters'
CREATE TABLE [dbo].[Submitters] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [LastName] nvarchar(max)  NULL,
    [FirstName] nvarchar(max)  NULL,
    [MiddleName] nvarchar(max)  NULL,
    [OrgName] nvarchar(max)  NULL,
    [Identifier] nvarchar(max)  NULL,
    [Phone] nvarchar(max)  NULL,
    [Email] nvarchar(max)  NULL,
    [Fax] nvarchar(max)  NULL,
    [ContactName] nvarchar(max)  NULL
);
GO

-- Creating table 'Receivers'
CREATE TABLE [dbo].[Receivers] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NULL,
    [Identifier] nvarchar(max)  NULL,
    [Active] bit  NULL
);
GO

-- Creating table 'Batches'
CREATE TABLE [dbo].[Batches] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [DateCreated] datetime  NULL,
    [Identifier] nvarchar(max)  NULL,
    [ClaimIDs] nvarchar(max)  NULL,
    [CreatedById] int  NOT NULL,
    [Status] nvarchar(max)  NULL,
    [SystemNoteKey] nvarchar(max)  NULL
);
GO

-- Creating table 'PayToes'
CREATE TABLE [dbo].[PayToes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Address_1] nvarchar(max)  NULL,
    [Address_2] nvarchar(max)  NULL,
    [City] nvarchar(max)  NULL,
    [State] nvarchar(max)  NULL,
    [Zip] nvarchar(max)  NULL,
    [Name] nvarchar(max)  NULL,
    [BillingProviderId] int  NULL,
    [RenderingProviderId] int  NULL,
    [IsCompany] bit  NOT NULL
);
GO

-- Creating table 'ClaimAccidents'
CREATE TABLE [dbo].[ClaimAccidents] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Causes] nvarchar(max)  NULL,
    [State] nvarchar(max)  NULL,
    [Country] nvarchar(max)  NULL,
    [Date] datetime  NULL
);
GO

-- Creating table 'ClaimSupplementals'
CREATE TABLE [dbo].[ClaimSupplementals] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [AttachmentType] nvarchar(max)  NULL,
    [TransmissionType] nvarchar(max)  NULL,
    [AttachmentNumber] nvarchar(max)  NULL
);
GO

-- Creating table 'ClaimContracts'
CREATE TABLE [dbo].[ClaimContracts] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Type] nvarchar(max)  NULL,
    [Amount] decimal(18,2)  NULL,
    [Percent] decimal(18,0)  NULL,
    [Code] nvarchar(max)  NULL,
    [Discount] nvarchar(max)  NULL,
    [Version] nvarchar(max)  NULL,
    [SystemNoteKey] nvarchar(max)  NULL
);
GO

-- Creating table 'ClaimReferences'
CREATE TABLE [dbo].[ClaimReferences] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [AuthorizationException] nvarchar(max)  NULL,
    [MandatoryMedicare] nvarchar(max)  NULL,
    [Mammography] nvarchar(max)  NULL,
    [Referral] nvarchar(max)  NULL,
    [Prior] nvarchar(max)  NULL,
    [ClaimControl] nvarchar(max)  NULL,
    [Repriced] nvarchar(max)  NULL,
    [AdjustedRepriced] nvarchar(max)  NULL,
    [InvestigationalDevice] nvarchar(max)  NULL,
    [Transmission] nvarchar(max)  NULL,
    [MedicalRecord] nvarchar(max)  NULL,
    [Demonstration] nvarchar(max)  NULL,
    [CarePlanOversight] nvarchar(max)  NULL,
    [CLIA] nvarchar(max)  NULL
);
GO

-- Creating table 'ClaimAmbulances'
CREATE TABLE [dbo].[ClaimAmbulances] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Weight] decimal(18,2)  NULL,
    [Reason] nvarchar(max)  NULL,
    [Quantity] decimal(18,0)  NULL,
    [RoundTrip] nvarchar(max)  NULL,
    [Stretcher] nvarchar(max)  NULL,
    [CertCode_1] nvarchar(max)  NULL,
    [CertCode_2] nvarchar(max)  NULL,
    [CertCode_3] nvarchar(max)  NULL,
    [CertCode_4] nvarchar(max)  NULL,
    [CertCode_5] nvarchar(max)  NULL,
    [CertResponse] nvarchar(max)  NULL,
    [PatientCount] int  NULL,
    [SystemNoteKey] nvarchar(max)  NULL
);
GO

-- Creating table 'ClaimSpinals'
CREATE TABLE [dbo].[ClaimSpinals] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Code] nvarchar(max)  NULL,
    [Description_1] nvarchar(max)  NULL,
    [Description_2] nvarchar(max)  NULL
);
GO

-- Creating table 'ClaimConditions'
CREATE TABLE [dbo].[ClaimConditions] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [EPSDT] nvarchar(max)  NULL,
    [Anesthesia] nvarchar(max)  NULL,
    [Conditions] nvarchar(max)  NULL,
    [Vision] nvarchar(max)  NULL
);
GO

-- Creating table 'ClaimRepricings'
CREATE TABLE [dbo].[ClaimRepricings] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Methodology] nvarchar(max)  NULL,
    [AllowedAmount] decimal(18,2)  NULL,
    [SavingAmount] decimal(18,2)  NULL,
    [OrgIdentifier] nvarchar(max)  NULL,
    [Rate] decimal(18,2)  NULL,
    [AmbulatoryGroup] nvarchar(max)  NULL,
    [AmbulatoryAmount] decimal(18,2)  NULL,
    [RejectReason] nvarchar(max)  NULL,
    [PolicyCompliance] nvarchar(max)  NULL,
    [Exception] nvarchar(max)  NULL
);
GO

-- Creating table 'ClaimAmounts'
CREATE TABLE [dbo].[ClaimAmounts] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [PayerPaid] decimal(18,2)  NULL,
    [NonCovered] decimal(18,2)  NULL,
    [PatientLiability] decimal(18,2)  NULL
);
GO

-- Creating table 'Insurers'
CREATE TABLE [dbo].[Insurers] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [SystemNoteKey] nvarchar(max)  NOT NULL,
    [FirstName] nvarchar(max)  NOT NULL,
    [LastName] nvarchar(max)  NOT NULL,
    [Address_1] nvarchar(max)  NOT NULL,
    [Address_2] nvarchar(max)  NOT NULL,
    [City] nvarchar(max)  NOT NULL,
    [State] nvarchar(max)  NOT NULL,
    [Zip] nvarchar(max)  NOT NULL,
    [Phone_1] nvarchar(max)  NOT NULL,
    [Phone_2] nvarchar(max)  NOT NULL,
    [Email] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'UserRoles'
CREATE TABLE [dbo].[UserRoles] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Inactive] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Accounts'
CREATE TABLE [dbo].[Accounts] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [DateCreated] datetime  NOT NULL,
    [IsInactive] bit  NOT NULL,
    [SystemNoteKey] nvarchar(max)  NULL
);
GO

-- Creating table 'Payments'
CREATE TABLE [dbo].[Payments] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [SystemNoteKey] nvarchar(max)  NULL,
    [DateCreated] datetime  NOT NULL,
    [Notes] nvarchar(max)  NULL,
    [AmountTotal] decimal(18,0)  NOT NULL,
    [AmountPayment] decimal(18,0)  NOT NULL,
    [AmountCopay] decimal(18,0)  NOT NULL,
    [AmountDeductible] decimal(18,0)  NOT NULL,
    [AmountBalance] decimal(18,0)  NOT NULL,
    [ErrorCode_Id] int  NULL
);
GO

-- Creating table 'PaymentLines'
CREATE TABLE [dbo].[PaymentLines] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [SystemNoteKey] nvarchar(max)  NOT NULL,
    [DateCreated] datetime  NOT NULL,
    [ClaimLineId] int  NULL,
    [AmountPayment] decimal(18,0)  NULL,
    [AmountCopay] decimal(18,0)  NULL,
    [AmountDeductible] decimal(18,0)  NULL,
    [PaymentId] int  NULL
);
GO

-- Creating table 'ErrorCodes'
CREATE TABLE [dbo].[ErrorCodes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Description] nvarchar(max)  NOT NULL,
    [Code] nvarchar(max)  NOT NULL,
    [ARTo] nvarchar(max)  NULL,
    [EditBatch] nvarchar(max)  NULL,
    [ToPayer] nvarchar(max)  NULL,
    [ToPatient] nvarchar(max)  NULL
);
GO

-- Creating table 'ErrorMessages'
CREATE TABLE [dbo].[ErrorMessages] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Message] nvarchar(max)  NOT NULL,
    [ErrorCodeErrorMessage_ErrorMessage_Id] int  NULL
);
GO

-- Creating table 'UserPreferences'
CREATE TABLE [dbo].[UserPreferences] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [UserId] int  NULL,
    [AccountId] int  NULL,
    [SystemNoteKey] nvarchar(max)  NULL,
    [RenderingProviderId] int  NULL,
    [BillingProviderId] int  NULL,
    [FacilityId] int  NULL,
    [PlaceOfServiceId] int  NULL
);
GO

-- Creating table 'AddressProvider'
CREATE TABLE [dbo].[AddressProvider] (
    [Addresses_Id] int  NOT NULL,
    [AddressProvider_Address_Id] int  NOT NULL
);
GO

-- Creating table 'AddressSubscriber'
CREATE TABLE [dbo].[AddressSubscriber] (
    [Addresses_Id] int  NOT NULL,
    [AddressSubscriber_Address_Id] int  NOT NULL
);
GO

-- Creating table 'PatientSubscriber'
CREATE TABLE [dbo].[PatientSubscriber] (
    [Patients_Id] int  NOT NULL,
    [PatientSubscriber_Patient_Id] int  NOT NULL
);
GO

-- Creating table 'ProviderProvider'
CREATE TABLE [dbo].[ProviderProvider] (
    [ProviderProvider_Provider1_Id] int  NOT NULL,
    [Providers_Id] int  NOT NULL
);
GO

-- Creating table 'UserAccount'
CREATE TABLE [dbo].[UserAccount] (
    [UserAccount_Account_Id] int  NOT NULL,
    [Accounts_Id] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'Addresses'
ALTER TABLE [dbo].[Addresses]
ADD CONSTRAINT [PK_Addresses]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ClaimLines'
ALTER TABLE [dbo].[ClaimLines]
ADD CONSTRAINT [PK_ClaimLines]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Claims'
ALTER TABLE [dbo].[Claims]
ADD CONSTRAINT [PK_Claims]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'CPTCodes'
ALTER TABLE [dbo].[CPTCodes]
ADD CONSTRAINT [PK_CPTCodes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Facilities'
ALTER TABLE [dbo].[Facilities]
ADD CONSTRAINT [PK_Facilities]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ICDCodes'
ALTER TABLE [dbo].[ICDCodes]
ADD CONSTRAINT [PK_ICDCodes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Payers'
ALTER TABLE [dbo].[Payers]
ADD CONSTRAINT [PK_Payers]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Modifiers'
ALTER TABLE [dbo].[Modifiers]
ADD CONSTRAINT [PK_Modifiers]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Patients'
ALTER TABLE [dbo].[Patients]
ADD CONSTRAINT [PK_Patients]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Providers'
ALTER TABLE [dbo].[Providers]
ADD CONSTRAINT [PK_Providers]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'PlaceOfServices'
ALTER TABLE [dbo].[PlaceOfServices]
ADD CONSTRAINT [PK_PlaceOfServices]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'TaxonomyCodes'
ALTER TABLE [dbo].[TaxonomyCodes]
ADD CONSTRAINT [PK_TaxonomyCodes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Subscribers'
ALTER TABLE [dbo].[Subscribers]
ADD CONSTRAINT [PK_Subscribers]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SystemNotes'
ALTER TABLE [dbo].[SystemNotes]
ADD CONSTRAINT [PK_SystemNotes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Users'
ALTER TABLE [dbo].[Users]
ADD CONSTRAINT [PK_Users]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ClaimDates'
ALTER TABLE [dbo].[ClaimDates]
ADD CONSTRAINT [PK_ClaimDates]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ClaimLineDMERCs'
ALTER TABLE [dbo].[ClaimLineDMERCs]
ADD CONSTRAINT [PK_ClaimLineDMERCs]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ClaimLineDates'
ALTER TABLE [dbo].[ClaimLineDates]
ADD CONSTRAINT [PK_ClaimLineDates]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ClaimLineReferences'
ALTER TABLE [dbo].[ClaimLineReferences]
ADD CONSTRAINT [PK_ClaimLineReferences]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ClaimLineSupplementals'
ALTER TABLE [dbo].[ClaimLineSupplementals]
ADD CONSTRAINT [PK_ClaimLineSupplementals]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ClaimLineDrugs'
ALTER TABLE [dbo].[ClaimLineDrugs]
ADD CONSTRAINT [PK_ClaimLineDrugs]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ClaimLineAttachments'
ALTER TABLE [dbo].[ClaimLineAttachments]
ADD CONSTRAINT [PK_ClaimLineAttachments]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ClaimLineDurables'
ALTER TABLE [dbo].[ClaimLineDurables]
ADD CONSTRAINT [PK_ClaimLineDurables]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ClaimLineAmbulances'
ALTER TABLE [dbo].[ClaimLineAmbulances]
ADD CONSTRAINT [PK_ClaimLineAmbulances]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ClaimLineContracts'
ALTER TABLE [dbo].[ClaimLineContracts]
ADD CONSTRAINT [PK_ClaimLineContracts]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ClaimLineDialysis'
ALTER TABLE [dbo].[ClaimLineDialysis]
ADD CONSTRAINT [PK_ClaimLineDialysis]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ClaimLinePricings'
ALTER TABLE [dbo].[ClaimLinePricings]
ADD CONSTRAINT [PK_ClaimLinePricings]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ClaimLineProducts'
ALTER TABLE [dbo].[ClaimLineProducts]
ADD CONSTRAINT [PK_ClaimLineProducts]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Submitters'
ALTER TABLE [dbo].[Submitters]
ADD CONSTRAINT [PK_Submitters]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Receivers'
ALTER TABLE [dbo].[Receivers]
ADD CONSTRAINT [PK_Receivers]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Batches'
ALTER TABLE [dbo].[Batches]
ADD CONSTRAINT [PK_Batches]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'PayToes'
ALTER TABLE [dbo].[PayToes]
ADD CONSTRAINT [PK_PayToes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ClaimAccidents'
ALTER TABLE [dbo].[ClaimAccidents]
ADD CONSTRAINT [PK_ClaimAccidents]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ClaimSupplementals'
ALTER TABLE [dbo].[ClaimSupplementals]
ADD CONSTRAINT [PK_ClaimSupplementals]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ClaimContracts'
ALTER TABLE [dbo].[ClaimContracts]
ADD CONSTRAINT [PK_ClaimContracts]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ClaimReferences'
ALTER TABLE [dbo].[ClaimReferences]
ADD CONSTRAINT [PK_ClaimReferences]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ClaimAmbulances'
ALTER TABLE [dbo].[ClaimAmbulances]
ADD CONSTRAINT [PK_ClaimAmbulances]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ClaimSpinals'
ALTER TABLE [dbo].[ClaimSpinals]
ADD CONSTRAINT [PK_ClaimSpinals]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ClaimConditions'
ALTER TABLE [dbo].[ClaimConditions]
ADD CONSTRAINT [PK_ClaimConditions]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ClaimRepricings'
ALTER TABLE [dbo].[ClaimRepricings]
ADD CONSTRAINT [PK_ClaimRepricings]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ClaimAmounts'
ALTER TABLE [dbo].[ClaimAmounts]
ADD CONSTRAINT [PK_ClaimAmounts]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Insurers'
ALTER TABLE [dbo].[Insurers]
ADD CONSTRAINT [PK_Insurers]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'UserRoles'
ALTER TABLE [dbo].[UserRoles]
ADD CONSTRAINT [PK_UserRoles]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Accounts'
ALTER TABLE [dbo].[Accounts]
ADD CONSTRAINT [PK_Accounts]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Payments'
ALTER TABLE [dbo].[Payments]
ADD CONSTRAINT [PK_Payments]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'PaymentLines'
ALTER TABLE [dbo].[PaymentLines]
ADD CONSTRAINT [PK_PaymentLines]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ErrorCodes'
ALTER TABLE [dbo].[ErrorCodes]
ADD CONSTRAINT [PK_ErrorCodes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ErrorMessages'
ALTER TABLE [dbo].[ErrorMessages]
ADD CONSTRAINT [PK_ErrorMessages]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'UserPreferences'
ALTER TABLE [dbo].[UserPreferences]
ADD CONSTRAINT [PK_UserPreferences]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Addresses_Id], [AddressProvider_Address_Id] in table 'AddressProvider'
ALTER TABLE [dbo].[AddressProvider]
ADD CONSTRAINT [PK_AddressProvider]
    PRIMARY KEY CLUSTERED ([Addresses_Id], [AddressProvider_Address_Id] ASC);
GO

-- Creating primary key on [Addresses_Id], [AddressSubscriber_Address_Id] in table 'AddressSubscriber'
ALTER TABLE [dbo].[AddressSubscriber]
ADD CONSTRAINT [PK_AddressSubscriber]
    PRIMARY KEY CLUSTERED ([Addresses_Id], [AddressSubscriber_Address_Id] ASC);
GO

-- Creating primary key on [Patients_Id], [PatientSubscriber_Patient_Id] in table 'PatientSubscriber'
ALTER TABLE [dbo].[PatientSubscriber]
ADD CONSTRAINT [PK_PatientSubscriber]
    PRIMARY KEY CLUSTERED ([Patients_Id], [PatientSubscriber_Patient_Id] ASC);
GO

-- Creating primary key on [ProviderProvider_Provider1_Id], [Providers_Id] in table 'ProviderProvider'
ALTER TABLE [dbo].[ProviderProvider]
ADD CONSTRAINT [PK_ProviderProvider]
    PRIMARY KEY CLUSTERED ([ProviderProvider_Provider1_Id], [Providers_Id] ASC);
GO

-- Creating primary key on [UserAccount_Account_Id], [Accounts_Id] in table 'UserAccount'
ALTER TABLE [dbo].[UserAccount]
ADD CONSTRAINT [PK_UserAccount]
    PRIMARY KEY CLUSTERED ([UserAccount_Account_Id], [Accounts_Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [ClaimLine_Id] in table 'ClaimLineDates'
ALTER TABLE [dbo].[ClaimLineDates]
ADD CONSTRAINT [FK_ClaimLineDateClaimLine]
    FOREIGN KEY ([ClaimLine_Id])
    REFERENCES [dbo].[ClaimLines]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ClaimLineDateClaimLine'
CREATE INDEX [IX_FK_ClaimLineDateClaimLine]
ON [dbo].[ClaimLineDates]
    ([ClaimLine_Id]);
GO

-- Creating foreign key on [ClaimLine_Id] in table 'ClaimLineReferences'
ALTER TABLE [dbo].[ClaimLineReferences]
ADD CONSTRAINT [FK_ClaimLineRefClaimLine]
    FOREIGN KEY ([ClaimLine_Id])
    REFERENCES [dbo].[ClaimLines]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ClaimLineRefClaimLine'
CREATE INDEX [IX_FK_ClaimLineRefClaimLine]
ON [dbo].[ClaimLineReferences]
    ([ClaimLine_Id]);
GO

-- Creating foreign key on [ClaimLine_Id] in table 'ClaimLineDrugs'
ALTER TABLE [dbo].[ClaimLineDrugs]
ADD CONSTRAINT [FK_ClaimLineDrugClaimLine]
    FOREIGN KEY ([ClaimLine_Id])
    REFERENCES [dbo].[ClaimLines]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ClaimLineDrugClaimLine'
CREATE INDEX [IX_FK_ClaimLineDrugClaimLine]
ON [dbo].[ClaimLineDrugs]
    ([ClaimLine_Id]);
GO

-- Creating foreign key on [ClaimLine_Id] in table 'ClaimLineAttachments'
ALTER TABLE [dbo].[ClaimLineAttachments]
ADD CONSTRAINT [FK_ClaimLineAttachmentClaimLine]
    FOREIGN KEY ([ClaimLine_Id])
    REFERENCES [dbo].[ClaimLines]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ClaimLineAttachmentClaimLine'
CREATE INDEX [IX_FK_ClaimLineAttachmentClaimLine]
ON [dbo].[ClaimLineAttachments]
    ([ClaimLine_Id]);
GO

-- Creating foreign key on [ClaimLine_Id] in table 'ClaimLineDurables'
ALTER TABLE [dbo].[ClaimLineDurables]
ADD CONSTRAINT [FK_ClaimLineDurableClaimLine]
    FOREIGN KEY ([ClaimLine_Id])
    REFERENCES [dbo].[ClaimLines]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ClaimLineDurableClaimLine'
CREATE INDEX [IX_FK_ClaimLineDurableClaimLine]
ON [dbo].[ClaimLineDurables]
    ([ClaimLine_Id]);
GO

-- Creating foreign key on [ClaimLine_Id] in table 'ClaimLineContracts'
ALTER TABLE [dbo].[ClaimLineContracts]
ADD CONSTRAINT [FK_ClaimLineContractClaimLine]
    FOREIGN KEY ([ClaimLine_Id])
    REFERENCES [dbo].[ClaimLines]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ClaimLineContractClaimLine'
CREATE INDEX [IX_FK_ClaimLineContractClaimLine]
ON [dbo].[ClaimLineContracts]
    ([ClaimLine_Id]);
GO

-- Creating foreign key on [ClaimLine_Id] in table 'ClaimLineDialysis'
ALTER TABLE [dbo].[ClaimLineDialysis]
ADD CONSTRAINT [FK_ClaimLineDialysisClaimLine]
    FOREIGN KEY ([ClaimLine_Id])
    REFERENCES [dbo].[ClaimLines]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ClaimLineDialysisClaimLine'
CREATE INDEX [IX_FK_ClaimLineDialysisClaimLine]
ON [dbo].[ClaimLineDialysis]
    ([ClaimLine_Id]);
GO

-- Creating foreign key on [ClaimLine_Id] in table 'ClaimLinePricings'
ALTER TABLE [dbo].[ClaimLinePricings]
ADD CONSTRAINT [FK_ClaimLinePricingClaimLine]
    FOREIGN KEY ([ClaimLine_Id])
    REFERENCES [dbo].[ClaimLines]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ClaimLinePricingClaimLine'
CREATE INDEX [IX_FK_ClaimLinePricingClaimLine]
ON [dbo].[ClaimLinePricings]
    ([ClaimLine_Id]);
GO

-- Creating foreign key on [ClaimLine_Id] in table 'ClaimLineProducts'
ALTER TABLE [dbo].[ClaimLineProducts]
ADD CONSTRAINT [FK_ClaimLineProductClaimLine]
    FOREIGN KEY ([ClaimLine_Id])
    REFERENCES [dbo].[ClaimLines]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ClaimLineProductClaimLine'
CREATE INDEX [IX_FK_ClaimLineProductClaimLine]
ON [dbo].[ClaimLineProducts]
    ([ClaimLine_Id]);
GO

-- Creating foreign key on [ClaimLine_Id] in table 'ClaimLineAmbulances'
ALTER TABLE [dbo].[ClaimLineAmbulances]
ADD CONSTRAINT [FK_ClaimLineAmbulanceClaimLine]
    FOREIGN KEY ([ClaimLine_Id])
    REFERENCES [dbo].[ClaimLines]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ClaimLineAmbulanceClaimLine'
CREATE INDEX [IX_FK_ClaimLineAmbulanceClaimLine]
ON [dbo].[ClaimLineAmbulances]
    ([ClaimLine_Id]);
GO

-- Creating foreign key on [ClaimLine_Id] in table 'ClaimLineSupplementals'
ALTER TABLE [dbo].[ClaimLineSupplementals]
ADD CONSTRAINT [FK_ClaimLineSupplementalClaimLine]
    FOREIGN KEY ([ClaimLine_Id])
    REFERENCES [dbo].[ClaimLines]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ClaimLineSupplementalClaimLine'
CREATE INDEX [IX_FK_ClaimLineSupplementalClaimLine]
ON [dbo].[ClaimLineSupplementals]
    ([ClaimLine_Id]);
GO

-- Creating foreign key on [PlaceOfServiceId] in table 'Claims'
ALTER TABLE [dbo].[Claims]
ADD CONSTRAINT [FK_PlaceOfServiceClaim]
    FOREIGN KEY ([PlaceOfServiceId])
    REFERENCES [dbo].[PlaceOfServices]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PlaceOfServiceClaim'
CREATE INDEX [IX_FK_PlaceOfServiceClaim]
ON [dbo].[Claims]
    ([PlaceOfServiceId]);
GO

-- Creating foreign key on [TaxonomyCodeId] in table 'Providers'
ALTER TABLE [dbo].[Providers]
ADD CONSTRAINT [FK_TaxonomyCodeProvider]
    FOREIGN KEY ([TaxonomyCodeId])
    REFERENCES [dbo].[TaxonomyCodes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_TaxonomyCodeProvider'
CREATE INDEX [IX_FK_TaxonomyCodeProvider]
ON [dbo].[Providers]
    ([TaxonomyCodeId]);
GO

-- Creating foreign key on [PrimaryPayerId] in table 'Subscribers'
ALTER TABLE [dbo].[Subscribers]
ADD CONSTRAINT [FK_PayerSubscriber]
    FOREIGN KEY ([PrimaryPayerId])
    REFERENCES [dbo].[Payers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PayerSubscriber'
CREATE INDEX [IX_FK_PayerSubscriber]
ON [dbo].[Subscribers]
    ([PrimaryPayerId]);
GO

-- Creating foreign key on [SecondaryPayerId] in table 'Subscribers'
ALTER TABLE [dbo].[Subscribers]
ADD CONSTRAINT [FK_PayerSubscriber1]
    FOREIGN KEY ([SecondaryPayerId])
    REFERENCES [dbo].[Payers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PayerSubscriber1'
CREATE INDEX [IX_FK_PayerSubscriber1]
ON [dbo].[Subscribers]
    ([SecondaryPayerId]);
GO

-- Creating foreign key on [PrimarySubscriberId] in table 'Claims'
ALTER TABLE [dbo].[Claims]
ADD CONSTRAINT [FK_SubscriberClaim]
    FOREIGN KEY ([PrimarySubscriberId])
    REFERENCES [dbo].[Subscribers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SubscriberClaim'
CREATE INDEX [IX_FK_SubscriberClaim]
ON [dbo].[Claims]
    ([PrimarySubscriberId]);
GO

-- Creating foreign key on [SecondarySubscriberId] in table 'Claims'
ALTER TABLE [dbo].[Claims]
ADD CONSTRAINT [FK_SubscriberClaim1]
    FOREIGN KEY ([SecondarySubscriberId])
    REFERENCES [dbo].[Subscribers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SubscriberClaim1'
CREATE INDEX [IX_FK_SubscriberClaim1]
ON [dbo].[Claims]
    ([SecondarySubscriberId]);
GO

-- Creating foreign key on [PatientId] in table 'Claims'
ALTER TABLE [dbo].[Claims]
ADD CONSTRAINT [FK_PatientClaim]
    FOREIGN KEY ([PatientId])
    REFERENCES [dbo].[Patients]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PatientClaim'
CREATE INDEX [IX_FK_PatientClaim]
ON [dbo].[Claims]
    ([PatientId]);
GO

-- Creating foreign key on [FacilityId] in table 'Claims'
ALTER TABLE [dbo].[Claims]
ADD CONSTRAINT [FK_FacilityClaim]
    FOREIGN KEY ([FacilityId])
    REFERENCES [dbo].[Facilities]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_FacilityClaim'
CREATE INDEX [IX_FK_FacilityClaim]
ON [dbo].[Claims]
    ([FacilityId]);
GO

-- Creating foreign key on [PrimaryPayerId] in table 'Claims'
ALTER TABLE [dbo].[Claims]
ADD CONSTRAINT [FK_PayerClaim]
    FOREIGN KEY ([PrimaryPayerId])
    REFERENCES [dbo].[Payers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PayerClaim'
CREATE INDEX [IX_FK_PayerClaim]
ON [dbo].[Claims]
    ([PrimaryPayerId]);
GO

-- Creating foreign key on [SecondaryPayerId] in table 'Claims'
ALTER TABLE [dbo].[Claims]
ADD CONSTRAINT [FK_PayerClaim1]
    FOREIGN KEY ([SecondaryPayerId])
    REFERENCES [dbo].[Payers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PayerClaim1'
CREATE INDEX [IX_FK_PayerClaim1]
ON [dbo].[Claims]
    ([SecondaryPayerId]);
GO

-- Creating foreign key on [ClaimId] in table 'ClaimLines'
ALTER TABLE [dbo].[ClaimLines]
ADD CONSTRAINT [FK_ClaimClaimLine]
    FOREIGN KEY ([ClaimId])
    REFERENCES [dbo].[Claims]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ClaimClaimLine'
CREATE INDEX [IX_FK_ClaimClaimLine]
ON [dbo].[ClaimLines]
    ([ClaimId]);
GO

-- Creating foreign key on [Dates_Id] in table 'Claims'
ALTER TABLE [dbo].[Claims]
ADD CONSTRAINT [FK_ClaimDateClaim]
    FOREIGN KEY ([Dates_Id])
    REFERENCES [dbo].[ClaimDates]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ClaimDateClaim'
CREATE INDEX [IX_FK_ClaimDateClaim]
ON [dbo].[Claims]
    ([Dates_Id]);
GO

-- Creating foreign key on [Addresses_Id] in table 'AddressProvider'
ALTER TABLE [dbo].[AddressProvider]
ADD CONSTRAINT [FK_AddressProvider_Address]
    FOREIGN KEY ([Addresses_Id])
    REFERENCES [dbo].[Addresses]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [AddressProvider_Address_Id] in table 'AddressProvider'
ALTER TABLE [dbo].[AddressProvider]
ADD CONSTRAINT [FK_AddressProvider_Provider]
    FOREIGN KEY ([AddressProvider_Address_Id])
    REFERENCES [dbo].[Providers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AddressProvider_Provider'
CREATE INDEX [IX_FK_AddressProvider_Provider]
ON [dbo].[AddressProvider]
    ([AddressProvider_Address_Id]);
GO

-- Creating foreign key on [Addresses_Id] in table 'AddressSubscriber'
ALTER TABLE [dbo].[AddressSubscriber]
ADD CONSTRAINT [FK_AddressSubscriber_Address]
    FOREIGN KEY ([Addresses_Id])
    REFERENCES [dbo].[Addresses]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [AddressSubscriber_Address_Id] in table 'AddressSubscriber'
ALTER TABLE [dbo].[AddressSubscriber]
ADD CONSTRAINT [FK_AddressSubscriber_Subscriber]
    FOREIGN KEY ([AddressSubscriber_Address_Id])
    REFERENCES [dbo].[Subscribers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AddressSubscriber_Subscriber'
CREATE INDEX [IX_FK_AddressSubscriber_Subscriber]
ON [dbo].[AddressSubscriber]
    ([AddressSubscriber_Address_Id]);
GO

-- Creating foreign key on [ClaimLineClaimLineDMERC_ClaimLineDMERC_Id] in table 'ClaimLineDMERCs'
ALTER TABLE [dbo].[ClaimLineDMERCs]
ADD CONSTRAINT [FK_ClaimLineClaimLineDMERC]
    FOREIGN KEY ([ClaimLineClaimLineDMERC_ClaimLineDMERC_Id])
    REFERENCES [dbo].[ClaimLines]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ClaimLineClaimLineDMERC'
CREATE INDEX [IX_FK_ClaimLineClaimLineDMERC]
ON [dbo].[ClaimLineDMERCs]
    ([ClaimLineClaimLineDMERC_ClaimLineDMERC_Id]);
GO

-- Creating foreign key on [RenderingProviderId] in table 'Claims'
ALTER TABLE [dbo].[Claims]
ADD CONSTRAINT [FK_ProviderClaim1]
    FOREIGN KEY ([RenderingProviderId])
    REFERENCES [dbo].[Providers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ProviderClaim1'
CREATE INDEX [IX_FK_ProviderClaim1]
ON [dbo].[Claims]
    ([RenderingProviderId]);
GO

-- Creating foreign key on [BillingProviderId] in table 'Claims'
ALTER TABLE [dbo].[Claims]
ADD CONSTRAINT [FK_ProviderClaim]
    FOREIGN KEY ([BillingProviderId])
    REFERENCES [dbo].[Providers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ProviderClaim'
CREATE INDEX [IX_FK_ProviderClaim]
ON [dbo].[Claims]
    ([BillingProviderId]);
GO

-- Creating foreign key on [Accident_Id] in table 'Claims'
ALTER TABLE [dbo].[Claims]
ADD CONSTRAINT [FK_ClaimAccidentClaim]
    FOREIGN KEY ([Accident_Id])
    REFERENCES [dbo].[ClaimAccidents]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ClaimAccidentClaim'
CREATE INDEX [IX_FK_ClaimAccidentClaim]
ON [dbo].[Claims]
    ([Accident_Id]);
GO

-- Creating foreign key on [Supplemental_Id] in table 'Claims'
ALTER TABLE [dbo].[Claims]
ADD CONSTRAINT [FK_ClaimSupplementalClaim]
    FOREIGN KEY ([Supplemental_Id])
    REFERENCES [dbo].[ClaimSupplementals]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ClaimSupplementalClaim'
CREATE INDEX [IX_FK_ClaimSupplementalClaim]
ON [dbo].[Claims]
    ([Supplemental_Id]);
GO

-- Creating foreign key on [Contract_Id] in table 'Claims'
ALTER TABLE [dbo].[Claims]
ADD CONSTRAINT [FK_ClaimContractClaim]
    FOREIGN KEY ([Contract_Id])
    REFERENCES [dbo].[ClaimContracts]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ClaimContractClaim'
CREATE INDEX [IX_FK_ClaimContractClaim]
ON [dbo].[Claims]
    ([Contract_Id]);
GO

-- Creating foreign key on [Reference_Id] in table 'Claims'
ALTER TABLE [dbo].[Claims]
ADD CONSTRAINT [FK_ClaimReferenceClaim]
    FOREIGN KEY ([Reference_Id])
    REFERENCES [dbo].[ClaimReferences]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ClaimReferenceClaim'
CREATE INDEX [IX_FK_ClaimReferenceClaim]
ON [dbo].[Claims]
    ([Reference_Id]);
GO

-- Creating foreign key on [Ambulance_Id] in table 'Claims'
ALTER TABLE [dbo].[Claims]
ADD CONSTRAINT [FK_ClaimAmbulanceClaim]
    FOREIGN KEY ([Ambulance_Id])
    REFERENCES [dbo].[ClaimAmbulances]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ClaimAmbulanceClaim'
CREATE INDEX [IX_FK_ClaimAmbulanceClaim]
ON [dbo].[Claims]
    ([Ambulance_Id]);
GO

-- Creating foreign key on [Spinal_Id] in table 'Claims'
ALTER TABLE [dbo].[Claims]
ADD CONSTRAINT [FK_ClaimSpinalClaim]
    FOREIGN KEY ([Spinal_Id])
    REFERENCES [dbo].[ClaimSpinals]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ClaimSpinalClaim'
CREATE INDEX [IX_FK_ClaimSpinalClaim]
ON [dbo].[Claims]
    ([Spinal_Id]);
GO

-- Creating foreign key on [Condition_Id] in table 'Claims'
ALTER TABLE [dbo].[Claims]
ADD CONSTRAINT [FK_ClaimConditionClaim]
    FOREIGN KEY ([Condition_Id])
    REFERENCES [dbo].[ClaimConditions]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ClaimConditionClaim'
CREATE INDEX [IX_FK_ClaimConditionClaim]
ON [dbo].[Claims]
    ([Condition_Id]);
GO

-- Creating foreign key on [Repricing_Id] in table 'Claims'
ALTER TABLE [dbo].[Claims]
ADD CONSTRAINT [FK_ClaimRepricingClaim]
    FOREIGN KEY ([Repricing_Id])
    REFERENCES [dbo].[ClaimRepricings]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ClaimRepricingClaim'
CREATE INDEX [IX_FK_ClaimRepricingClaim]
ON [dbo].[Claims]
    ([Repricing_Id]);
GO

-- Creating foreign key on [Amounts_Id] in table 'Claims'
ALTER TABLE [dbo].[Claims]
ADD CONSTRAINT [FK_ClaimAmountClaim]
    FOREIGN KEY ([Amounts_Id])
    REFERENCES [dbo].[ClaimAmounts]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ClaimAmountClaim'
CREATE INDEX [IX_FK_ClaimAmountClaim]
ON [dbo].[Claims]
    ([Amounts_Id]);
GO

-- Creating foreign key on [Patients_Id] in table 'PatientSubscriber'
ALTER TABLE [dbo].[PatientSubscriber]
ADD CONSTRAINT [FK_PatientSubscriber_Patient]
    FOREIGN KEY ([Patients_Id])
    REFERENCES [dbo].[Patients]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [PatientSubscriber_Patient_Id] in table 'PatientSubscriber'
ALTER TABLE [dbo].[PatientSubscriber]
ADD CONSTRAINT [FK_PatientSubscriber_Subscriber]
    FOREIGN KEY ([PatientSubscriber_Patient_Id])
    REFERENCES [dbo].[Subscribers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PatientSubscriber_Subscriber'
CREATE INDEX [IX_FK_PatientSubscriber_Subscriber]
ON [dbo].[PatientSubscriber]
    ([PatientSubscriber_Patient_Id]);
GO

-- Creating foreign key on [ProviderProvider_Provider1_Id] in table 'ProviderProvider'
ALTER TABLE [dbo].[ProviderProvider]
ADD CONSTRAINT [FK_ProviderProvider_Provider]
    FOREIGN KEY ([ProviderProvider_Provider1_Id])
    REFERENCES [dbo].[Providers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Providers_Id] in table 'ProviderProvider'
ALTER TABLE [dbo].[ProviderProvider]
ADD CONSTRAINT [FK_ProviderProvider_Provider1]
    FOREIGN KEY ([Providers_Id])
    REFERENCES [dbo].[Providers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ProviderProvider_Provider1'
CREATE INDEX [IX_FK_ProviderProvider_Provider1]
ON [dbo].[ProviderProvider]
    ([Providers_Id]);
GO

-- Creating foreign key on [ErrorCodeErrorMessage_ErrorMessage_Id] in table 'ErrorMessages'
ALTER TABLE [dbo].[ErrorMessages]
ADD CONSTRAINT [FK_ErrorCodeErrorMessage]
    FOREIGN KEY ([ErrorCodeErrorMessage_ErrorMessage_Id])
    REFERENCES [dbo].[ErrorCodes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ErrorCodeErrorMessage'
CREATE INDEX [IX_FK_ErrorCodeErrorMessage]
ON [dbo].[ErrorMessages]
    ([ErrorCodeErrorMessage_ErrorMessage_Id]);
GO

-- Creating foreign key on [ErrorCode_Id] in table 'Payments'
ALTER TABLE [dbo].[Payments]
ADD CONSTRAINT [FK_ErrorCodePayment]
    FOREIGN KEY ([ErrorCode_Id])
    REFERENCES [dbo].[ErrorCodes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ErrorCodePayment'
CREATE INDEX [IX_FK_ErrorCodePayment]
ON [dbo].[Payments]
    ([ErrorCode_Id]);
GO

-- Creating foreign key on [Payment_Id] in table 'Claims'
ALTER TABLE [dbo].[Claims]
ADD CONSTRAINT [FK_PaymentClaim]
    FOREIGN KEY ([Payment_Id])
    REFERENCES [dbo].[Payments]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PaymentClaim'
CREATE INDEX [IX_FK_PaymentClaim]
ON [dbo].[Claims]
    ([Payment_Id]);
GO

-- Creating foreign key on [PaymentId] in table 'PaymentLines'
ALTER TABLE [dbo].[PaymentLines]
ADD CONSTRAINT [FK_PaymentPaymentLine]
    FOREIGN KEY ([PaymentId])
    REFERENCES [dbo].[Payments]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PaymentPaymentLine'
CREATE INDEX [IX_FK_PaymentPaymentLine]
ON [dbo].[PaymentLines]
    ([PaymentId]);
GO

-- Creating foreign key on [UserAccount_Account_Id] in table 'UserAccount'
ALTER TABLE [dbo].[UserAccount]
ADD CONSTRAINT [FK_UserAccount_User]
    FOREIGN KEY ([UserAccount_Account_Id])
    REFERENCES [dbo].[Users]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Accounts_Id] in table 'UserAccount'
ALTER TABLE [dbo].[UserAccount]
ADD CONSTRAINT [FK_UserAccount_Account]
    FOREIGN KEY ([Accounts_Id])
    REFERENCES [dbo].[Accounts]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserAccount_Account'
CREATE INDEX [IX_FK_UserAccount_Account]
ON [dbo].[UserAccount]
    ([Accounts_Id]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------
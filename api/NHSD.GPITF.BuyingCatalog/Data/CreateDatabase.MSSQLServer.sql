-- assumes database collation is:
--    Latin1_General_CI_AI

-- remove problematic foreign keys, so we can drop tables
IF OBJECT_ID(N'Contacts', N'U') IS NOT NULL
BEGIN
  BEGIN TRANSACTION
  ALTER TABLE Contacts DROP CONSTRAINT IF EXISTS FK_Contacts_Organisations
  COMMIT
END

IF OBJECT_ID(N'Solutions', N'U') IS NOT NULL
BEGIN
  BEGIN TRANSACTION
  ALTER TABLE Solutions DROP CONSTRAINT IF EXISTS FK_Solutions_Solutions
  ALTER TABLE Solutions DROP CONSTRAINT IF EXISTS FK_Solutions_Organisations
  ALTER TABLE Solutions DROP CONSTRAINT IF EXISTS FK_Solutions_CreatedBy
  ALTER TABLE Solutions DROP CONSTRAINT IF EXISTS FK_Solutions_ModifiedBy
  COMMIT
END


-- drop relationship tables
DROP TABLE IF EXISTS CapabilityFramework;
DROP TABLE IF EXISTS FrameworkSolution;
DROP TABLE IF EXISTS FrameworkStandard;
DROP TABLE IF EXISTS CapabilityStandard;

DROP TABLE IF EXISTS CapabilitiesImplementedReviews;
DROP TABLE IF EXISTS CapabilitiesImplementedEvidence;
DROP TABLE IF EXISTS CapabilitiesImplemented;

DROP TABLE IF EXISTS StandardsApplicableReviews;
DROP TABLE IF EXISTS StandardsApplicableEvidence;
DROP TABLE IF EXISTS StandardsApplicable;

-- drop data tables
DROP TABLE IF EXISTS TechnicalContacts;
DROP TABLE IF EXISTS Contacts;
DROP TABLE IF EXISTS Solutions;
DROP TABLE IF EXISTS Organisations;

DROP TABLE IF EXISTS Capabilities;
DROP TABLE IF EXISTS Frameworks;
DROP TABLE IF EXISTS Standards;

DROP TABLE IF EXISTS Log;

-- create data tables
-- NOTE:  maximum text field lengths is 425 characters because 
--        max index size (on relationship tables) is 1700 bytes (425 = 1700/4)

-- Organisations.tsv
CREATE TABLE Organisations
(
  Id NVARCHAR(36) NOT NULL UNIQUE,
  Name NVARCHAR(425) NOT NULL UNIQUE,
  PrimaryRoleId NVARCHAR(MAX) NOT NULL DEFAULT 'RO92', 
  Status NVARCHAR(MAX) NOT NULL DEFAULT 'Active',
  Description NVARCHAR(MAX),
  PRIMARY KEY (Id)
);

-- Contacts.tsv
CREATE TABLE Contacts
(
  Id NVARCHAR(36) NOT NULL UNIQUE,
  OrganisationId NVARCHAR(36) NOT NULL,
  FirstName NVARCHAR(MAX),
  LastName NVARCHAR(MAX),
  EmailAddress1 NVARCHAR(425) NOT NULL UNIQUE,
  PhoneNumber1 NVARCHAR(MAX),
  CONSTRAINT FK_Contacts_Organisations FOREIGN KEY (OrganisationId) REFERENCES Organisations(Id) ON DELETE NO ACTION,
  PRIMARY KEY (Id)
);

-- Solutions.tsv
CREATE TABLE Solutions
(
  Id NVARCHAR(36) NOT NULL UNIQUE,
  PreviousId NVARCHAR(36),
  OrganisationId NVARCHAR(36) NOT NULL,
  Version NVARCHAR(MAX) NOT NULL DEFAULT '',
  Status INTEGER DEFAULT 0,
  CreatedById NVARCHAR(36) NOT NULL,
  CreatedOn NVARCHAR(MAX) NOT NULL,
  ModifiedById NVARCHAR(36) NOT NULL,
  ModifiedOn NVARCHAR(MAX) NOT NULL,
  Name NVARCHAR(MAX) NOT NULL,
  Description NVARCHAR(MAX),
  ProductPage NVARCHAR(MAX),
  CONSTRAINT FK_Solutions_Solutions FOREIGN KEY (PreviousId) REFERENCES Solutions(Id),
  CONSTRAINT FK_Solutions_Organisations FOREIGN KEY (OrganisationId) REFERENCES Organisations(Id) ON DELETE CASCADE,
  CONSTRAINT FK_Solutions_CreatedBy FOREIGN KEY (CreatedById) REFERENCES Contacts(Id) ON DELETE NO ACTION,
  CONSTRAINT FK_Solutions_ModifiedBy FOREIGN KEY (ModifiedById) REFERENCES Contacts(Id) ON DELETE NO ACTION,
  PRIMARY KEY (Id)
);

-- TechnicalContacts.tsv
CREATE TABLE TechnicalContacts
(
  Id NVARCHAR(36) NOT NULL UNIQUE,
  SolutionId NVARCHAR(36) NOT NULL,
  ContactType NVARCHAR(MAX) NOT NULL,
  FirstName NVARCHAR(MAX),
  LastName NVARCHAR(MAX),
  EmailAddress NVARCHAR(MAX) NOT NULL,
  PhoneNumber NVARCHAR(MAX),
  FOREIGN KEY (SolutionId) REFERENCES Solutions(Id) ON DELETE CASCADE,
  PRIMARY KEY (Id)
);

-- Capabilities.tsv
CREATE TABLE Capabilities
(
  Id NVARCHAR(36) NOT NULL UNIQUE,
  PreviousId NVARCHAR(36),
  Name NVARCHAR(MAX) NOT NULL,
  Description NVARCHAR(MAX),
  URL NVARCHAR(MAX),
  Type NVARCHAR(MAX),
  PRIMARY KEY (Id),
  FOREIGN KEY (PreviousId) REFERENCES Capabilities(Id)
);

-- Frameworks.tsv
CREATE TABLE Frameworks
(
  Id NVARCHAR(36) NOT NULL UNIQUE,
  PreviousId NVARCHAR(36),
  Name NVARCHAR(MAX) NOT NULL,
  Description NVARCHAR(MAX),
  PRIMARY KEY (Id),
  FOREIGN KEY (PreviousId) REFERENCES Frameworks(Id)
);

-- Standards.tsv
CREATE TABLE Standards
(
  Id NVARCHAR(36) NOT NULL UNIQUE,
  PreviousId NVARCHAR(36),
  IsOverarching INTEGER DEFAULT 0,
  Name NVARCHAR(MAX) NOT NULL,
  Description NVARCHAR(MAX),
  URL NVARCHAR(MAX),
  Type NVARCHAR(MAX),
  PRIMARY KEY (Id),
  FOREIGN KEY (PreviousId) REFERENCES Standards(Id)
);

CREATE TABLE Log 
(
  Timestamp DATETIME2,
  Loglevel TEXT,
  Callsite TEXT,
  Message TEXT
);
CREATE INDEX IDX_Timestamp ON Log(Timestamp);


-- create relationship tables

-- CapabilitiesImplemented.tsv
CREATE TABLE CapabilitiesImplemented
(
  Id NVARCHAR(36) NOT NULL UNIQUE,
  SolutionId NVARCHAR(36) NOT NULL,
  CapabilityId NVARCHAR(36) NOT NULL,
  Status INTEGER DEFAULT 0,
  OwnerId NVARCHAR(36) DEFAULT NULL,
  OriginalDate NVARCHAR(MAX) NOT NULL,
  FOREIGN KEY (SolutionId) REFERENCES Solutions(Id) ON DELETE CASCADE,
  FOREIGN KEY (CapabilityId) REFERENCES Capabilities(Id) ON DELETE CASCADE,
  FOREIGN KEY (OwnerId) REFERENCES Contacts(Id) ON DELETE NO ACTION,
  PRIMARY KEY (Id)
);

-- StandardsApplicable.tsv
CREATE TABLE StandardsApplicable
(
  Id NVARCHAR(36) NOT NULL UNIQUE,
  SolutionId NVARCHAR(36) NOT NULL,
  StandardId NVARCHAR(36) NOT NULL,
  Status INTEGER DEFAULT 0,
  OwnerId NVARCHAR(36) DEFAULT NULL,
  OriginalDate NVARCHAR(MAX) NOT NULL,
  SubmittedOn NVARCHAR(MAX) NOT NULL,
  AssignedOn NVARCHAR(MAX) NOT NULL,
  FOREIGN KEY (SolutionId) REFERENCES Solutions(Id) ON DELETE CASCADE,
  FOREIGN KEY (StandardId) REFERENCES Standards(Id) ON DELETE CASCADE,
  FOREIGN KEY (OwnerId) REFERENCES Contacts(Id) ON DELETE NO ACTION,
  PRIMARY KEY (Id)
);

-- CapabilityFramework.tsv
CREATE TABLE CapabilityFramework
(
  CapabilityId NVARCHAR(36) NOT NULL,
  FrameworkId NVARCHAR(36) NOT NULL,
  FOREIGN KEY (CapabilityId) REFERENCES Capabilities(Id) ON DELETE NO ACTION,
  FOREIGN KEY (FrameworkId) REFERENCES Frameworks(Id) ON DELETE NO ACTION,
  PRIMARY KEY (CapabilityId, FrameworkId)
);

-- FrameworkSolution.tsv
CREATE TABLE FrameworkSolution
(
  FrameworkId NVARCHAR(36) NOT NULL,
  SolutionId NVARCHAR(36) NOT NULL,
  FOREIGN KEY (FrameworkId) REFERENCES Frameworks(Id) ON DELETE CASCADE,
  FOREIGN KEY (SolutionId) REFERENCES Solutions(Id) ON DELETE CASCADE,
  PRIMARY KEY (FrameworkId, SolutionId)
);

-- FrameworkStandard.tsv
CREATE TABLE FrameworkStandard
(
  FrameworkId NVARCHAR(36) NOT NULL,
  StandardId NVARCHAR(36) NOT NULL,
  FOREIGN KEY (FrameworkId) REFERENCES Frameworks(Id) ON DELETE NO ACTION,
  FOREIGN KEY (StandardId) REFERENCES Standards(Id) ON DELETE NO ACTION,
  PRIMARY KEY (FrameworkId, StandardId)
);

-- CapabilityStandard.tsv
CREATE TABLE CapabilityStandard
(
  CapabilityId NVARCHAR(36) NOT NULL,
  StandardId NVARCHAR(36) NOT NULL,
  IsOptional INTEGER DEFAULT 0,
  FOREIGN KEY (CapabilityId) REFERENCES Capabilities(Id) ON DELETE NO ACTION,
  FOREIGN KEY (StandardId) REFERENCES Standards(Id) ON DELETE NO ACTION,
  PRIMARY KEY (CapabilityId, StandardId)
);

-- CapabilitiesImplementedEvidence.tsv
CREATE TABLE CapabilitiesImplementedEvidence
(
  Id NVARCHAR(36) NOT NULL UNIQUE,
  PreviousId NVARCHAR(36),
  ClaimId NVARCHAR(36) NOT NULL,
  CreatedById NVARCHAR(36) NOT NULL,
  CreatedOn NVARCHAR(MAX) NOT NULL,
  OriginalDate NVARCHAR(MAX) NOT NULL,
  Evidence NVARCHAR(MAX),
  HasRequestedLiveDemo INTEGER DEFAULT 0,
  BlobId NVARCHAR(MAX),
  FOREIGN KEY (PreviousId) REFERENCES CapabilitiesImplementedEvidence(Id),
  FOREIGN KEY (ClaimId) REFERENCES CapabilitiesImplemented(Id) ON DELETE CASCADE,
  FOREIGN KEY (CreatedById) REFERENCES Contacts(Id) ON DELETE CASCADE,
  PRIMARY KEY (Id)
);

-- CapabilitiesImplementedReviews.tsv
CREATE TABLE CapabilitiesImplementedReviews
(
  Id NVARCHAR(36) NOT NULL UNIQUE,
  PreviousId NVARCHAR(36),
  EvidenceId NVARCHAR(36) NOT NULL,
  CreatedById NVARCHAR(36) NOT NULL,
  CreatedOn NVARCHAR(MAX) NOT NULL,
  OriginalDate NVARCHAR(MAX) NOT NULL,
  Message NVARCHAR(MAX),
  FOREIGN KEY (PreviousId) REFERENCES CapabilitiesImplementedReviews(Id),
  FOREIGN KEY (EvidenceId) REFERENCES CapabilitiesImplementedEvidence(Id) ON DELETE CASCADE,
  FOREIGN KEY (CreatedById) REFERENCES Contacts(Id) ON DELETE NO ACTION,
  PRIMARY KEY (Id)
);

-- StandardsApplicableEvidence.tsv
CREATE TABLE StandardsApplicableEvidence
(
  Id NVARCHAR(36) NOT NULL UNIQUE,
  PreviousId NVARCHAR(36),
  ClaimId NVARCHAR(36) NOT NULL,
  CreatedById NVARCHAR(36) NOT NULL,
  CreatedOn NVARCHAR(MAX) NOT NULL,
  OriginalDate NVARCHAR(MAX) NOT NULL,
  Evidence NVARCHAR(MAX),
  HasRequestedLiveDemo INTEGER DEFAULT 0,
  BlobId NVARCHAR(MAX),
  FOREIGN KEY (PreviousId) REFERENCES StandardsApplicableEvidence(Id),
  FOREIGN KEY (ClaimId) REFERENCES StandardsApplicable(Id) ON DELETE CASCADE,
  FOREIGN KEY (CreatedById) REFERENCES Contacts(Id) ON DELETE CASCADE,
  PRIMARY KEY (Id)
);

-- StandardsApplicableReviews.tsv
CREATE TABLE StandardsApplicableReviews
(
  Id NVARCHAR(36) NOT NULL UNIQUE,
  PreviousId NVARCHAR(36),
  EvidenceId NVARCHAR(36) NOT NULL,
  CreatedById NVARCHAR(36) NOT NULL,
  CreatedOn NVARCHAR(MAX) NOT NULL,
  OriginalDate NVARCHAR(MAX) NOT NULL,
  Message NVARCHAR(MAX),
  FOREIGN KEY (PreviousId) REFERENCES StandardsApplicableReviews(Id),
  FOREIGN KEY (EvidenceId) REFERENCES StandardsApplicableEvidence(Id) ON DELETE CASCADE,
  FOREIGN KEY (CreatedById) REFERENCES Contacts(Id) ON DELETE NO ACTION,
  PRIMARY KEY (Id)
);




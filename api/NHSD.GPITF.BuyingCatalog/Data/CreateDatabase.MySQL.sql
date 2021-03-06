-- assumes database collation is:
--    utf8_unicode_ci

-- disable foreign key checking so we can delete tables
SET FOREIGN_KEY_CHECKS = 0;

-- drop relationship tables
DROP TABLE IF EXISTS CapabilityFramework;
DROP TABLE IF EXISTS FrameworkSolution;
DROP TABLE IF EXISTS FrameworkStandard;
DROP TABLE IF EXISTS CapabilityStandard;
DROP TABLE IF EXISTS CapabilitiesImplemented;
DROP TABLE IF EXISTS StandardsApplicable;
DROP TABLE IF EXISTS CapabilitiesImplementedEvidence;
DROP TABLE IF EXISTS CapabilitiesImplementedReviews;
DROP TABLE IF EXISTS StandardsApplicableEvidence;
DROP TABLE IF EXISTS StandardsApplicableReviews;

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

-- Organisations.tsv
CREATE TABLE Organisations
(
  Id CHAR(38) NOT NULL UNIQUE,
  Name VARCHAR(512) NOT NULL UNIQUE,
  PrimaryRoleId TEXT NOT NULL DEFAULT 'RO92', 
  Status TEXT NOT NULL DEFAULT 'Active',
  Description TEXT,
  PRIMARY KEY (Id)
);

-- Contacts.tsv
CREATE TABLE Contacts
(
  Id CHAR(38) NOT NULL UNIQUE,
  OrganisationId CHAR(38) NOT NULL,
  FirstName TEXT,
  LastName TEXT,
  EmailAddress1 CHAR(128) NOT NULL UNIQUE,
  PhoneNumber1 TEXT,
  FOREIGN KEY (OrganisationId) REFERENCES Organisations(Id) ON DELETE CASCADE,
  PRIMARY KEY (Id)
);

-- Solutions.tsv
CREATE TABLE Solutions
(
  Id CHAR(38) NOT NULL UNIQUE,
  PreviousId CHAR(38),
  OrganisationId CHAR(38) NOT NULL,
  Version TEXT NOT NULL DEFAULT '',
  Status INTEGER DEFAULT 0,
  CreatedById CHAR(38) NOT NULL,
  CreatedOn TEXT NOT NULL,
  ModifiedById CHAR(38) NOT NULL,
  ModifiedOn TEXT NOT NULL,
  Name TEXT NOT NULL,
  Description TEXT,
  ProductPage TEXT,
  FOREIGN KEY (PreviousId) REFERENCES Solutions(Id),
  FOREIGN KEY (OrganisationId) REFERENCES Organisations(Id) ON DELETE CASCADE,
  FOREIGN KEY (CreatedById) REFERENCES Contacts(Id) ON DELETE CASCADE,
  FOREIGN KEY (ModifiedById) REFERENCES Contacts(Id) ON DELETE CASCADE,
  PRIMARY KEY (Id)
);

-- TechnicalContacts.tsv
CREATE TABLE TechnicalContacts
(
  Id CHAR(38) NOT NULL UNIQUE,
  SolutionId CHAR(38) NOT NULL,
  ContactType TEXT NOT NULL,
  FirstName TEXT,
  LastName TEXT,
  EmailAddress TEXT NOT NULL,
  PhoneNumber TEXT,
  FOREIGN KEY (SolutionId) REFERENCES Solutions(Id) ON DELETE CASCADE,
  PRIMARY KEY (Id)
);

-- Capabilities.tsv
CREATE TABLE Capabilities
(
  Id CHAR(38) NOT NULL UNIQUE,
  PreviousId CHAR(38),
  Name TEXT NOT NULL,
  Description TEXT,
  URL TEXT,
  Type TEXT,
  PRIMARY KEY (Id),
  FOREIGN KEY (PreviousId) REFERENCES Capabilities(Id)
);

-- Frameworks.tsv
CREATE TABLE Frameworks
(
  Id CHAR(38) NOT NULL UNIQUE,
  PreviousId CHAR(38),
  Name TEXT NOT NULL,
  Description TEXT,
  PRIMARY KEY (Id),
  FOREIGN KEY (PreviousId) REFERENCES Frameworks(Id)
);

-- Standards.tsv
CREATE TABLE Standards
(
  Id CHAR(38) NOT NULL UNIQUE,
  PreviousId CHAR(38),
  IsOverarching INTEGER DEFAULT 0,
  Name TEXT NOT NULL,
  Description TEXT,
  URL TEXT,
  Type TEXT,
  PRIMARY KEY (Id),
  FOREIGN KEY (PreviousId) REFERENCES Standards(Id)
);

CREATE TABLE Log 
(
  Timestamp DATETIME,
  Loglevel TEXT,
  Callsite TEXT,
  Message TEXT
);
CREATE INDEX IDX_Timestamp ON Log(Timestamp);


-- create relationship tables

-- CapabilitiesImplemented.tsv
CREATE TABLE CapabilitiesImplemented
(
  Id CHAR(38) NOT NULL UNIQUE,
  SolutionId CHAR(38) NOT NULL,
  CapabilityId CHAR(38) NOT NULL,
  Status INTEGER DEFAULT 0,
  OwnerId CHAR(38),
  OriginalDate TEXT NOT NULL,
  FOREIGN KEY (SolutionId) REFERENCES Solutions(Id) ON DELETE CASCADE,
  FOREIGN KEY (CapabilityId) REFERENCES Capabilities(Id) ON DELETE CASCADE,
  FOREIGN KEY (OwnerId) REFERENCES Contacts(Id) ON DELETE CASCADE,
  PRIMARY KEY (Id)
);

-- StandardsApplicable.tsv
CREATE TABLE StandardsApplicable
(
  Id CHAR(38) NOT NULL UNIQUE,
  SolutionId CHAR(38) NOT NULL,
  StandardId CHAR(38) NOT NULL,
  Status INTEGER DEFAULT 0,
  OwnerId CHAR(38),
  OriginalDate TEXT NOT NULL,
  SubmittedOn TEXT NOT NULL,
  AssignedOn TEXT NOT NULL,
  FOREIGN KEY (SolutionId) REFERENCES Solutions(Id) ON DELETE CASCADE,
  FOREIGN KEY (StandardId) REFERENCES Standards(Id) ON DELETE CASCADE,
  FOREIGN KEY (OwnerId) REFERENCES Contacts(Id) ON DELETE CASCADE,
  PRIMARY KEY (Id)
);

-- CapabilityFramework.tsv
CREATE TABLE CapabilityFramework
(
  CapabilityId CHAR(38) NOT NULL,
  FrameworkId CHAR(38) NOT NULL,
  FOREIGN KEY (CapabilityId) REFERENCES Capabilities(Id) ON DELETE NO ACTION,
  FOREIGN KEY (FrameworkId) REFERENCES Frameworks(Id) ON DELETE NO ACTION,
  PRIMARY KEY (CapabilityId, FrameworkId)
);

-- FrameworkSolution.tsv
CREATE TABLE FrameworkSolution
(
  FrameworkId CHAR(38) NOT NULL,
  SolutionId CHAR(38) NOT NULL,
  FOREIGN KEY (FrameworkId) REFERENCES Frameworks(Id) ON DELETE CASCADE,
  FOREIGN KEY (SolutionId) REFERENCES Solutions(Id) ON DELETE CASCADE,
  PRIMARY KEY (FrameworkId, SolutionId)
);

-- FrameworkStandard.tsv
CREATE TABLE FrameworkStandard
(
  FrameworkId CHAR(38) NOT NULL,
  StandardId CHAR(38) NOT NULL,
  FOREIGN KEY (FrameworkId) REFERENCES Frameworks(Id) ON DELETE NO ACTION,
  FOREIGN KEY (StandardId) REFERENCES Standards(Id) ON DELETE NO ACTION,
  PRIMARY KEY (FrameworkId, StandardId)
);

-- CapabilityStandard.tsv
CREATE TABLE CapabilityStandard
(
  CapabilityId CHAR(38) NOT NULL,
  StandardId CHAR(38) NOT NULL,
  IsOptional INTEGER DEFAULT 0,
  FOREIGN KEY (CapabilityId) REFERENCES Capabilities(Id) ON DELETE NO ACTION,
  FOREIGN KEY (StandardId) REFERENCES Standards(Id) ON DELETE NO ACTION,
  PRIMARY KEY (CapabilityId, StandardId)
);

-- CapabilitiesImplementedEvidence.tsv
CREATE TABLE CapabilitiesImplementedEvidence
(
  Id CHAR(38) NOT NULL UNIQUE,
  PreviousId CHAR(38),
  ClaimId CHAR(38) NOT NULL,
  CreatedById CHAR(38) NOT NULL,
  CreatedOn TEXT NOT NULL,
  OriginalDate TEXT NOT NULL,
  Evidence TEXT,
  HasRequestedLiveDemo INTEGER DEFAULT 0,
  BlobId TEXT,
  FOREIGN KEY (PreviousId) REFERENCES CapabilitiesImplementedEvidence(Id),
  FOREIGN KEY (ClaimId) REFERENCES CapabilitiesImplemented(Id) ON DELETE CASCADE,
  FOREIGN KEY (CreatedById) REFERENCES Contacts(Id) ON DELETE CASCADE,
  PRIMARY KEY (Id)
);

-- CapabilitiesImplementedReviews.tsv
CREATE TABLE CapabilitiesImplementedReviews
(
  Id CHAR(38) NOT NULL UNIQUE,
  PreviousId CHAR(38),
  EvidenceId CHAR(38) NOT NULL,
  CreatedById CHAR(38) NOT NULL,
  CreatedOn TEXT NOT NULL,
  OriginalDate TEXT NOT NULL,
  Message TEXT,
  FOREIGN KEY (PreviousId) REFERENCES CapabilitiesImplementedReviews(Id),
  FOREIGN KEY (EvidenceId) REFERENCES CapabilitiesImplementedEvidence(Id) ON DELETE CASCADE,
  FOREIGN KEY (CreatedById) REFERENCES Contacts(Id) ON DELETE CASCADE,
  PRIMARY KEY (Id)
);

-- StandardsApplicableEvidence.tsv
CREATE TABLE StandardsApplicableEvidence
(
  Id CHAR(38) NOT NULL UNIQUE,
  PreviousId CHAR(38),
  ClaimId CHAR(38) NOT NULL,
  CreatedById CHAR(38) NOT NULL,
  CreatedOn TEXT NOT NULL,
  OriginalDate TEXT NOT NULL,
  Evidence TEXT,
  HasRequestedLiveDemo INTEGER DEFAULT 0,
  BlobId TEXT,
  FOREIGN KEY (PreviousId) REFERENCES StandardsApplicableEvidence(Id),
  FOREIGN KEY (ClaimId) REFERENCES StandardsApplicable(Id) ON DELETE CASCADE,
  FOREIGN KEY (CreatedById) REFERENCES Contacts(Id) ON DELETE CASCADE,
  PRIMARY KEY (Id)
);

-- StandardsApplicableReviews.tsv
CREATE TABLE StandardsApplicableReviews
(
  Id CHAR(38) NOT NULL UNIQUE,
  PreviousId CHAR(38),
  EvidenceId CHAR(38) NOT NULL,
  CreatedById CHAR(38) NOT NULL,
  CreatedOn TEXT NOT NULL,
  OriginalDate TEXT NOT NULL,
  Message TEXT,
  FOREIGN KEY (PreviousId) REFERENCES StandardsApplicableReviews(Id),
  FOREIGN KEY (EvidenceId) REFERENCES StandardsApplicableEvidence(Id) ON DELETE CASCADE,
  FOREIGN KEY (CreatedById) REFERENCES Contacts(Id) ON DELETE CASCADE,
  PRIMARY KEY (Id)
);

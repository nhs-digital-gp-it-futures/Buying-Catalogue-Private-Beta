-- assumes database collation is:
--    utf8_unicode_ci

-- NOTE:  table names are not quoted & PostgreSQL will lowercase these
--        column names *are* quoted & PostgreSQL will preserve case
--        This is done to support Dapper based data importer,
--        along with specific workarounds in the importer

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
DROP TABLE IF EXISTS Contacts CASCADE;
DROP TABLE IF EXISTS Solutions;
DROP TABLE IF EXISTS Organisations;

DROP TABLE IF EXISTS Capabilities;
DROP TABLE IF EXISTS Frameworks;
DROP TABLE IF EXISTS Standards;

DROP TABLE IF EXISTS Log;

-- create data tables

-- Organisations.csv
CREATE TABLE Organisations
(
  "Id" CHAR(36) NOT NULL UNIQUE,
  "Name" VARCHAR(512) NOT NULL UNIQUE,
  "PrimaryRoleId" TEXT NOT NULL DEFAULT 'RO92', 
  "Status" TEXT NOT NULL DEFAULT 'Active',
  "Description" TEXT,
  PRIMARY KEY ("Id")
);

-- Contacts.csv
CREATE TABLE Contacts
(
  "Id" CHAR(36) NOT NULL UNIQUE,
  "OrganisationId" CHAR(36) NOT NULL,
  "FirstName" TEXT,
  "LastName" TEXT,
  "EmailAddress1" VARCHAR(128) NOT NULL UNIQUE,
  "PhoneNumber1" TEXT,
  FOREIGN KEY ("OrganisationId") REFERENCES Organisations("Id") ON DELETE CASCADE,
  PRIMARY KEY ("Id")
);

-- Solutions.csv
CREATE TABLE Solutions
(
  "Id" CHAR(36) NOT NULL UNIQUE,
  "PreviousId" CHAR(36),
  "OrganisationId" CHAR(36) NOT NULL,
  "Version" TEXT NOT NULL DEFAULT '',
  "Status" INTEGER DEFAULT 0,
  "CreatedById" CHAR(36) NOT NULL,
  "CreatedOn" TEXT NOT NULL,
  "ModifiedById" CHAR(36) NOT NULL,
  "ModifiedOn" TEXT NOT NULL,
  "Name" TEXT NOT NULL,
  "Description" TEXT,
  "ProductPage" TEXT,
  FOREIGN KEY ("PreviousId") REFERENCES Solutions("Id"),
  FOREIGN KEY ("OrganisationId") REFERENCES Organisations("Id") ON DELETE CASCADE,
  FOREIGN KEY ("CreatedById") REFERENCES Contacts("Id") ON DELETE CASCADE,
  FOREIGN KEY ("ModifiedById") REFERENCES Contacts("Id") ON DELETE CASCADE,
  PRIMARY KEY ("Id")
);

-- TechnicalContacts.csv
CREATE TABLE TechnicalContacts
(
  "Id" CHAR(36) NOT NULL UNIQUE,
  "SolutionId" CHAR(36) NOT NULL,
  "ContactType" TEXT NOT NULL,
  "FirstName" TEXT,
  "LastName" TEXT,
  "EmailAddress" TEXT NOT NULL,
  "PhoneNumber" TEXT,
  FOREIGN KEY ("SolutionId") REFERENCES Solutions("Id") ON DELETE CASCADE,
  PRIMARY KEY ("Id")
);

-- Capabilities.csv
CREATE TABLE Capabilities
(
  "Id" CHAR(36) NOT NULL UNIQUE,
  "PreviousId" CHAR(36),
  "Name" TEXT NOT NULL,
  "Description" TEXT,
  "URL" TEXT,
  "Type" TEXT,
  PRIMARY KEY ("Id"),
  FOREIGN KEY ("PreviousId") REFERENCES Capabilities("Id")
);

-- Frameworks.csv
CREATE TABLE Frameworks
(
  "Id" CHAR(36) NOT NULL UNIQUE,
  "PreviousId" CHAR(36),
  "Name" TEXT NOT NULL,
  "Description" TEXT,
  PRIMARY KEY ("Id"),
  FOREIGN KEY ("PreviousId") REFERENCES Frameworks("Id")
);

-- Standards.csv
CREATE TABLE Standards
(
  "Id" CHAR(36) NOT NULL UNIQUE,
  "PreviousId" CHAR(36),
  "IsOverarching" BOOLEAN DEFAULT false,
  "Name" TEXT NOT NULL,
  "Description" TEXT,
  "URL" TEXT,
  "Type" TEXT,
  PRIMARY KEY ("Id"),
  FOREIGN KEY ("PreviousId") REFERENCES Standards("Id")
);

CREATE TABLE Log 
(
  "Timestamp" TIMESTAMP,
  "Loglevel" TEXT,
  "Callsite" TEXT,
  "Message" TEXT
);
CREATE INDEX IDX_Timestamp ON Log("Timestamp");


-- create relationship tables

-- CapabilitiesImplemented.csv
CREATE TABLE CapabilitiesImplemented
(
  "Id" CHAR(36) NOT NULL UNIQUE,
  "SolutionId" CHAR(36) NOT NULL,
  "CapabilityId" CHAR(36) NOT NULL,
  "Status" INTEGER DEFAULT 0,
  "OwnerId" CHAR(36),
  "CreatedOn" TEXT NOT NULL,
  FOREIGN KEY ("SolutionId") REFERENCES Solutions("Id") ON DELETE CASCADE,
  FOREIGN KEY ("CapabilityId") REFERENCES Capabilities("Id") ON DELETE CASCADE,
  FOREIGN KEY ("OwnerId") REFERENCES Contacts("Id") ON DELETE CASCADE,
  PRIMARY KEY ("Id")
);

-- StandardsApplicable.csv
CREATE TABLE StandardsApplicable
(
  "Id" CHAR(36) NOT NULL UNIQUE,
  "SolutionId" CHAR(36) NOT NULL,
  "StandardId" CHAR(36) NOT NULL,
  "Status" INTEGER DEFAULT 0,
  "OwnerId" CHAR(36),
  "CreatedOn" TEXT NOT NULL,
  "SubmittedOn" TEXT NOT NULL,
  "AssignedOn" TEXT NOT NULL,
  FOREIGN KEY ("SolutionId") REFERENCES Solutions("Id") ON DELETE CASCADE,
  FOREIGN KEY ("StandardId") REFERENCES Standards("Id") ON DELETE CASCADE,
  FOREIGN KEY ("OwnerId") REFERENCES Contacts("Id") ON DELETE CASCADE,
  PRIMARY KEY ("Id")
);

-- CapabilityFramework.csv
CREATE TABLE CapabilityFramework
(
  "CapabilityId" CHAR(36) NOT NULL,
  "FrameworkId" CHAR(36) NOT NULL,
  FOREIGN KEY ("CapabilityId") REFERENCES Capabilities("Id") ON DELETE NO ACTION,
  FOREIGN KEY ("FrameworkId") REFERENCES Frameworks("Id") ON DELETE NO ACTION,
  PRIMARY KEY ("CapabilityId", "FrameworkId")
);

-- FrameworkSolution.csv
CREATE TABLE FrameworkSolution
(
  "FrameworkId" CHAR(36) NOT NULL,
  "SolutionId" CHAR(36) NOT NULL,
  FOREIGN KEY ("FrameworkId") REFERENCES Frameworks("Id") ON DELETE CASCADE,
  FOREIGN KEY ("SolutionId") REFERENCES Solutions("Id") ON DELETE CASCADE,
  PRIMARY KEY ("FrameworkId", "SolutionId")
);

-- FrameworkStandard.csv
CREATE TABLE FrameworkStandard
(
  "FrameworkId" CHAR(36) NOT NULL,
  "StandardId" CHAR(36) NOT NULL,
  FOREIGN KEY ("FrameworkId") REFERENCES Frameworks("Id") ON DELETE NO ACTION,
  FOREIGN KEY ("StandardId") REFERENCES Standards("Id") ON DELETE NO ACTION,
  PRIMARY KEY ("FrameworkId", "StandardId")
);

-- CapabilityStandard.csv
CREATE TABLE CapabilityStandard
(
  "CapabilityId" CHAR(36) NOT NULL,
  "StandardId" CHAR(36) NOT NULL,
  "IsOptional" BOOLEAN DEFAULT false,
  FOREIGN KEY ("CapabilityId") REFERENCES Capabilities("Id") ON DELETE NO ACTION,
  FOREIGN KEY ("StandardId") REFERENCES Standards("Id") ON DELETE NO ACTION,
  PRIMARY KEY ("CapabilityId", "StandardId")
);

-- CapabilitiesImplementedEvidence.csv
CREATE TABLE CapabilitiesImplementedEvidence
(
  "Id" CHAR(36) NOT NULL UNIQUE,
  "PreviousId" CHAR(36),
  "ClaimId" CHAR(36) NOT NULL,
  "CreatedById" CHAR(36) NOT NULL,
  "CreatedOn" TEXT NOT NULL,
  "OriginalDate" TEXT NOT NULL,
  "Evidence" TEXT,
  "HasRequestedLiveDemo" BOOLEAN DEFAULT false,
  "BlobId" TEXT,
  FOREIGN KEY ("PreviousId") REFERENCES CapabilitiesImplementedEvidence("Id"),
  FOREIGN KEY ("ClaimId") REFERENCES CapabilitiesImplemented("Id") ON DELETE CASCADE,
  FOREIGN KEY ("CreatedById") REFERENCES Contacts("Id") ON DELETE CASCADE,
  PRIMARY KEY ("Id")
);

-- CapabilitiesImplementedReviews.csv
CREATE TABLE CapabilitiesImplementedReviews
(
  "Id" CHAR(36) NOT NULL UNIQUE,
  "PreviousId" CHAR(36),
  "EvidenceId" CHAR(36) NOT NULL,
  "CreatedById" CHAR(36) NOT NULL,
  "CreatedOn" TEXT NOT NULL,
  "OriginalDate" TEXT NOT NULL,
  "Message" TEXT,
  FOREIGN KEY ("PreviousId") REFERENCES CapabilitiesImplementedReviews("Id"),
  FOREIGN KEY ("EvidenceId") REFERENCES CapabilitiesImplementedEvidence("Id") ON DELETE CASCADE,
  FOREIGN KEY ("CreatedById") REFERENCES Contacts("Id") ON DELETE CASCADE,
  PRIMARY KEY ("Id")
);

-- StandardsApplicableEvidence.csv
CREATE TABLE StandardsApplicableEvidence
(
  "Id" CHAR(36) NOT NULL UNIQUE,
  "PreviousId" CHAR(36),
  "ClaimId" CHAR(36) NOT NULL,
  "CreatedById" CHAR(36) NOT NULL,
  "CreatedOn" TEXT NOT NULL,
  "OriginalDate" TEXT NOT NULL,
  "Evidence" TEXT,
  "HasRequestedLiveDemo" BOOLEAN DEFAULT false,
  "BlobId" TEXT,
  FOREIGN KEY ("PreviousId") REFERENCES StandardsApplicableEvidence("Id"),
  FOREIGN KEY ("ClaimId") REFERENCES StandardsApplicable("Id") ON DELETE CASCADE,
  FOREIGN KEY ("CreatedById") REFERENCES Contacts("Id") ON DELETE CASCADE,
  PRIMARY KEY ("Id")
);

-- StandardsApplicableReviews.csv
CREATE TABLE StandardsApplicableReviews
(
  "Id" CHAR(36) NOT NULL UNIQUE,
  "PreviousId" CHAR(36),
  "EvidenceId" CHAR(36) NOT NULL,
  "CreatedById" CHAR(36) NOT NULL,
  "CreatedOn" TEXT NOT NULL,
  "OriginalDate" TEXT NOT NULL,
  "Message" TEXT,
  FOREIGN KEY ("PreviousId") REFERENCES StandardsApplicableReviews("Id"),
  FOREIGN KEY ("EvidenceId") REFERENCES StandardsApplicableEvidence("Id") ON DELETE CASCADE,
  FOREIGN KEY ("CreatedById") REFERENCES Contacts("Id") ON DELETE CASCADE,
  PRIMARY KEY ("Id")
);

.read Preimport.SQLite.sql
.mode csv
.separator "\t"
.import Organisations.tsv Organisations
.import Contacts.tsv Contacts
.import Solutions.tsv Solutions
.import TechnicalContacts.tsv TechnicalContacts
.import Capabilities.tsv Capabilities
.import Frameworks.tsv Frameworks
.import Standards.tsv Standards
.import CapabilitiesImplemented.tsv CapabilitiesImplemented
.import StandardsApplicable.tsv StandardsApplicable
.import CapabilityFramework.tsv CapabilityFramework
.import FrameworkSolution.tsv FrameworkSolution
.import FrameworkStandard.tsv FrameworkStandard
.import CapabilityStandard.tsv CapabilityStandard
.import CapabilitiesImplementedEvidence.tsv CapabilitiesImplementedEvidence
.import CapabilitiesImplementedReviews.tsv CapabilitiesImplementedReviews
.import StandardsApplicableEvidence.tsv StandardsApplicableEvidence
.import StandardsApplicableReviews.tsv StandardsApplicableReviews
.read Postimport.SQLite.sql

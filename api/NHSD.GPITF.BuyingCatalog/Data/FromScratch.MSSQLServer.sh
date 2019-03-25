#! /bin/sh
TP=/opt/mssql-tools/bin
CREDS="-S localhost -U sa -P SA@Password"
BCP=$TP/bcp
SQL=$TP/sqlcmd

$SQL $CREDS -Q 'CREATE DATABASE BuyingCatalog COLLATE Latin1_General_CI_AI'

$SQL $CREDS -Q 'CREATE LOGIN NHSD WITH PASSWORD="DisruptTheMarket", CHECK_POLICY=OFF'
$SQL $CREDS -d BuyingCatalog -Q 'CREATE USER NHSD FOR LOGIN NHSD'
$SQL $CREDS -d BuyingCatalog -Q 'GRANT CONTROL ON SCHEMA::dbo TO NHSD'

$SQL $CREDS -d BuyingCatalog -i CreateDatabase.MSSQLServer.sql

import()
{
  TABLENAME=$1
  CSVFILE=\'$2\'
  IMPORT="$BCP $TABLENAME in $CSVFILE $CREDS -F 2 -d BuyingCatalog -c"
  echo
  echo ---
  wc -l "$2"
  echo ---
  eval "$IMPORT"
}

import Organisations 'Organisations.tsv'
import Contacts 'Contacts.tsv'
import Solutions 'Solutions.tsv'
import TechnicalContacts 'TechnicalContacts.tsv'
import Capabilities 'Capabilities.tsv'
import Frameworks 'Frameworks.tsv'
import Standards 'Standards.tsv'
import CapabilitiesImplemented 'CapabilitiesImplemented.tsv'
import StandardsApplicable 'StandardsApplicable.tsv'
import CapabilityFramework 'CapabilityFramework.tsv'
import FrameworkSolution 'FrameworkSolution.tsv'
import FrameworkStandard 'FrameworkStandard.tsv'
import CapabilityStandard 'CapabilityStandard.tsv'
import CapabilitiesImplementedEvidence 'CapabilitiesImplementedEvidence.tsv'
import CapabilitiesImplementedReviews 'CapabilitiesImplementedReviews.tsv'
import StandardsApplicableEvidence 'StandardsApplicableEvidence.tsv'
import StandardsApplicableReviews 'StandardsApplicableReviews.tsv'

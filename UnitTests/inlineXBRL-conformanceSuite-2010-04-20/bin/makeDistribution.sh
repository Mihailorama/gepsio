#!/bin/bash

#
# We assume that all scripts are run by make
# which means that they are run from the parent
# directory which contains the Makefile
#

. bin/CONFIG

# Process test case source files

# Get the date of the specification, as we need it for the file name 
# This means that changes to the conformance suite will inherit the 
# date from the current version of the specification, which isn't 
# ideal...

echo "Checking specification date..."
SPECDATE=`xsltproc "${BIN_DIR}"/date.xsl "${SPECIFICATION_DIR}"/inlineXBRL-part1-spec.xml`

echo -n "Creating distribution as a zip file"
pushd "${TEST_ROOT_DIR}"/.. > /dev/null
zip -r conf-processor/inlineXBRL-conformanceSuite-"${SPECDATE}".zip conf-processor -x \*svn\* -x \*extra\*
popd  > /dev/null

# We don't make any attempt to copy this file to the pub or nopub location but
# this could be added in here if we needed to.

echo ""

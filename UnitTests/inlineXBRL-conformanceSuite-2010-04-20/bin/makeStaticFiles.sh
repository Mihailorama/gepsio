#!/bin/bash

#
# We assume that all scripts are run by make
# which means that they are run from the parent
# directory which contains the Makefile
#

. bin/CONFIG

# Process test case source files

echo -n "Creating ixbrl (html), information and predictedOutput files"
pushd "${EXTRACTORTEST_DATA_DIR}" > /dev/null

for i in *.example ; do
  echo -n "."
  export name=`echo -n "$i" | sed s/\\\..*//`
  xsltproc "${BIN_DIR}/extractTestInputs.xsl"  $i > "${OUTPUT_DIR}/${name}.html"
  xsltproc "${BIN_DIR}/extractTestOutputs.xsl" $i > "${OUTPUT_DIR}/${name}.predictedOutput"

  xsltproc --stringparam schemaFilename "${IXBRLSCHEMA_DIR}/xhtml-inlinexbrl-0_1.xsd" \
           --stringparam baseFilename "${name}" \
           "${BIN_DIR}"/extractTestMetadata.xsl "$i" \
           > "${OUTPUT_DIR}/${name}.xml" 
done

echo ""

popd > /dev/null

# Get the date of the specification, as we need to insert it into index.xml 

echo "Checking specification date..."
SPECDATE=`xsltproc "${BIN_DIR}"/date.xsl "${SPECIFICATION_DIR}"/inlineXBRL-part1-spec.xml`

# Build index.xml file
# Copy header

echo "Creating index..."
cat "${XML_FRAGMENT_DIR}"/index_top.xml | sed s/SPECDATE/${SPECDATE}/ > "${OUTPUT_INDEX_DIR}"/index.xml

# Create testcases stanza for extractor test cases

sed s/TITLE/XBRL\ extraction\ test\ cases/ "${XML_FRAGMENT_DIR}"/index_testcases_top.xml | \
  sed s%ROOT%"${OUTPUT_REL_DIR}"% >> "${OUTPUT_INDEX_DIR}"/index.xml

pushd "${OUTPUT_DIR}" > /dev/null

for i in *.xml ; do
  sed s%URI%"${i}"% "${XML_FRAGMENT_DIR}"/index_testcase.xml \
    >> "${OUTPUT_INDEX_DIR}"/index.xml
done

popd > /dev/null

# Copy footer

cat "${XML_FRAGMENT_DIR}"/index_bottom.xml >> "${OUTPUT_INDEX_DIR}"/index.xml

# Copy stylesheets

echo "Copying stylesheets..."
cp "${STYLESHEET_SOURCE_DIR}"/* "${OUTPUT_STYLESHEET_DIR}"/.

# Copy schemas

echo "Copying schemas..."
cp -r --force "${SCHEMA_SOURCE_DIR}"/* "${OUTPUT_SCHEMA_DIR}"/.
cp    --force "${SCHEMA_GENERIC_SOURCE_DIR}"/*.xsd "${OUTPUT_SCHEMA_DIR}"/.

echo ""

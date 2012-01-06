#!/bin/bash

#
# We assume that all scripts are run by make
# which means that they are run from the parent
# directory which contains the Makefile
#

. bin/CONFIG

# Extract xbrl from html test case files

echo ""
echo "Running selected iXBRL extractor..."
pushd "${OUTPUT_DIR}" > /dev/null

for i in *.html ; do
  export n=`echo -n $i | sed s/\\\..*//`
  export x="${n}.xbrl"
  echo "PROCESSING ${i} TO GENERATE ${x}"
  java -jar "${PROCESSOR}" "${i}" "${IXBRL_EXTRACTOR}" > "${x}"
done

popd > /dev/null

echo ""

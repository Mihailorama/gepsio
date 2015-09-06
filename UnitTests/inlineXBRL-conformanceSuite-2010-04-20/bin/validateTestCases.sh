#!/bin/bash

#
# We assume that this will be run
# through the Makefile
#

. bin/CONFIG

# Abort if JAVA_HOME is not set
if [ -z "${JAVA_HOME}" ] ; then
  echo "${0}: Please ensure that $JAVA_HOME is set"
  exit 1
fi

let brokenCount=0

# Schema passcases

pushd ${OUTPUT_DIR}

for i in *.xml ; do
  export x=`echo -n $i | sed s/\\\..*//`
  echo "Purpose of test:"
  grep description "$i" | sed s/^.*\<description\>// | sed s/\<.description\>$//
  xercesValidateRec "${OUTPUT_DIR}/${x}.html"
  if [ `echo ${i} | cut -b -4` == PASS ] ;
    then
      if [ "$?" != "0" ];
      then
        let brokenCount=$brokenCount+1
      fi
    else
      if [ "$?" == "0" ];
      then
        let brokenCount=$brokenCount+1
      fi
  fi
  xercesValidateRec "${OUTPUT_DIR}/${x}.predictedOutput"
  if [ `echo ${i} | cut -b -4` == PASS ] ;
    then
      if [ "$?" != "0" ];
      then
        let brokenCount=$brokenCount+1
      fi
    else
      if [ "$?" == "0" ];
      then
        let brokenCount=$brokenCount+1
      fi
  fi
done

popd

echo "Broken test count: $brokenCount"

if [ "$brokenCount" -gt 254 ] ; then
  # don't exit with a wrapped return code ...
  brokenCount=1
fi

exit $brokenCount

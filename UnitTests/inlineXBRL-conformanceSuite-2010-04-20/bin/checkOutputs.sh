#!/bin/bash
#
# return code: 0 if the same
#              1 if different
#              N some other problem

. bin/CONFIG

ORIG_PATH=$PATH
# Importing functions sanitizes the PATH.  This is unhelpful because some
# utilities (like xmlpp) may not be on the sanitized PATH.
. /etc/init.d/functions
export PATH=$ORIG_PATH

let brokenCount=0

for i in "${OUTPUT_DIR}"/*.xbrl ; do
  export x=`echo -n $i | sed s/\\\.xbrl//`
  echo ""
  echo "Test case: ${x}.xbrl"
  echo -n "Purpose of test: "
  grep description "${x}.xml" | sed s/^.*\<description\>// | sed s/\<.description\>$//
  OUTPUT1=`mktemp XXXXXXXX`
  OUTPUT2=`mktemp XXXXXXXX`
  java -jar ../spec/common/bin/saxon9.jar -o ${OUTPUT1}.tmp "${i}" "${BIN_DIR}"/normalise.xsl
  java -jar ../spec/common/bin/saxon9.jar -o ${OUTPUT2}.tmp "${x}".predictedOutput "${BIN_DIR}"/normalise.xsl
  "${XMLPP}" -te ${OUTPUT1}.tmp > ${OUTPUT1}
  "${XMLPP}" -te ${OUTPUT2}.tmp > ${OUTPUT2}
  diff ${OUTPUT1} ${OUTPUT2} ;let DIFF_RESULT=$?
  rm -f ${OUTPUT1} ${OUTPUT2} ${OUTPUT1}.tmp ${OUTPUT2}.tmp

  if [ "${DIFF_RESULT}" != "0" ];
    then
      let brokenCount=$brokenCount+1
      failure
    else
      success
  fi
  echo ""
done

echo $brokenCount
if [ "$brokenCount" -gt 254 ] ; then
  brokenCount=1
fi

exit $brokenCount

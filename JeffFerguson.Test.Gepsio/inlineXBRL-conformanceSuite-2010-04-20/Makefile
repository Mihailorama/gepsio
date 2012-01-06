#!/bin/bash

include bin/CONFIG

all:					evaluate

setup:	
							@"${BIN_DIR}"/makeStaticFiles.sh

validate:			setup
							@"${BIN_DIR}"/validateTestCases.sh

extract:			setup
						 	@"${BIN_DIR}"/makeXBRL.sh

evaluate:			extract
							@"${BIN_DIR}"/checkOutputs.sh

zip:					clean
							@"${BIN_DIR}"/makeDistribution.sh

clean:				
							@if ls "${OUTPUT_INDEX_DIR}"/*.xml 2>/dev/null ; then rm "${OUTPUT_INDEX_DIR}"/*.xml ; fi
							@if ls "${OUTPUT_DIR}"/* 2>/dev/null ; then rm "${OUTPUT_DIR}"/* ; fi
							@if ls "${OUTPUT_STYLESHEET_DIR}"/* 2>/dev/null ; then rm "${OUTPUT_STYLESHEET_DIR}"/* ; fi
							@if ls "${OUTPUT_SCHEMA_DIR}"/* 2>/dev/null ; then rm -rf "${OUTPUT_SCHEMA_DIR}"/* ; fi
							@if ls "${TEST_ROOT_DIR}"/*.zip 2>/dev/null ; then rm "${TEST_ROOT_DIR}"/*.zip ; fi

help:			
							@echo ""
							@echo "make               Run complete suite"
							@echo "make setup         Set up test files"
							@echo "make validate      Check the test files"
							@echo "make extract       Run extractor"
							@echo "make evaluate      Evaluate results"
							@echo "make zip           Create distribution"
							@echo ""
							@echo "make clean         Delete all generated test cases"
							@echo "make help          This help message"
							@echo ""


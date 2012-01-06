<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
    <xsl:output method="xml" indent="yes"/>
    <xsl:param name="baseFilename" />
    <xsl:param name="schemaFilename" />
    <xsl:param name="expectedResult" />
    <xsl:template match="testcase/head">
        <xsl:processing-instruction name="xml-stylesheet">
            <xsl:text>type="text/xsl" href="../stylesheets/test.xsl"</xsl:text>
        </xsl:processing-instruction>
    <testcase
        xmlns="http://xbrl.org/2008/conformance"
        xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
        xmlns:xbrlfe="http://xbrl.org/2008/formula/errors"
        xmlns:binding="http://xbrl.org/2008/binding/errors"
        xmlns:label="http://xbrl.org/2008/label/errors"
        xmlns:reference="http://xbrl.org/2008/reference/errors"  
        xsi:schemaLocation="http://xbrl.org/2008/conformance ../schemas/test.xsd">
        <creator>
          <name><xsl:value-of select="creator/name" /></name>
          <email><xsl:value-of select="creator/email" /></email> 
        </creator>
        <number><xsl:value-of select="number"/></number>
        <name><xsl:value-of select="$baseFilename"/></name>
        <xsl:element name="reference">
            <xsl:attribute name="specification">IXBRL</xsl:attribute>
            <xsl:attribute name="id"><xsl:value-of select="requirement"/></xsl:attribute>
        </xsl:element>
 
        <variation>
            <description><xsl:value-of select="description"/></description>
            <data>
                <instance readMeFirst="true"><xsl:value-of select="concat($baseFilename, '.html')"/></instance>
                <schema readMeFirst="false"><xsl:value-of select="$schemaFilename"/></schema>
            </data>
        </variation>
    </testcase>
        
    </xsl:template>
    
    <xsl:template match="text()"/>
        
</xsl:stylesheet>

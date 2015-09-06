<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
    <xsl:output method="xml" omit-xml-declaration="yes" />
    <xsl:template match="//body/transformedOutput">
        <xsl:copy-of select="* | comment()"/>
    </xsl:template>
    
   <xsl:template match="text()"/>
    
    <xsl:template match="comment()/text()">
        <xsl:apply-templates />
    </xsl:template>
   
</xsl:stylesheet>

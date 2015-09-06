<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="2.0">
  <xsl:output method="xml" omit-xml-declaration="yes" indent="yes" />

  <xsl:template match="/">
    <xsl:copy-of copy-namespaces="no" select="." />
  </xsl:template>
    
  <xsl:template match="text()"/>

</xsl:stylesheet>

<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0"
  xmlns:s='http://xbrl.org/specification/2007'>

  <xsl:output method="text" />

  <xsl:template match="/">
    <xsl:value-of select="s:spec/s:header/s:date/@year" />
    <xsl:text>-</xsl:text>
    <xsl:value-of select="s:spec/s:header/s:date/@month" />
    <xsl:text>-</xsl:text>
    <xsl:value-of select="s:spec/s:header/s:date/@day" />
  </xsl:template>

</xsl:stylesheet>

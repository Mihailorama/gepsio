<?xml version="1.0" encoding="UTF-8"?>
<!-- Copyright 2007 XBRL International. All Rights Reserved. -->
<xsl:stylesheet version="1.0"
  xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
  xmlns:case="http://xbrl.org/2008/conformance"
  xmlns:eg="http://xbrl.org/2005/example">
  <xsl:template match="documentation">
    <html>
      <head>
        <title>
          <xsl:value-of select="@name" />
        </title>
      </head>
      <body>
        <h1>
          <xsl:value-of select="@name" />: <xsl:value-of select="@date" />
        </h1>
        <xsl:apply-templates select="testcases" />
        <xsl:apply-templates select="examples" />
      </body>
    </html>
  </xsl:template>
  
  <xsl:template match="testcases | performancetests | examples | datasets">

    <h2><xsl:value-of select="@title"/></h2>

    <table border="solid" cellpadding="5">
      <tbody valign="top">
        <tr>
          <th>Test</th>
          <th >Name</th>
          <th >Owner</th>
          <th >Description</th>
        </tr>
        <!-- now just generate one row per test set -->
        <xsl:apply-templates select="testcase | example" />
      </tbody>
    </table>
  </xsl:template>

  <xsl:template match="testcase | example">
    <xsl:variable name="uri" select="concat(../@root,'/',@uri)" />
    <xsl:variable name="case" select="document($uri,/.)/case:testcase" />
    <tr>
      <td align="right">
        <xsl:value-of select="$case/case:number" />
      </td>
      <td>
        <a href="{$uri}">
          <xsl:value-of select="$case/case:name" />
        </a>
      </td>
      <td>
        <xsl:element name="a">
          <xsl:attribute name="href">
            mailto: <xsl:value-of select="$case/case:creator/case:email" />
          </xsl:attribute>
          <xsl:value-of select="$case/case:creator/case:name" />
        </xsl:element>
      </td>
      <td>
        <xsl:value-of select="$case/case:variation/case:description" />
      </td>
    </tr>
  </xsl:template>
  
</xsl:stylesheet>

<?xml version="1.0" encoding="UTF-8"?>
<!-- Copyright 2007 XBRL International. All Rights Reserved. -->
<xsl:stylesheet 
  version="1.0" 
  xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
  xmlns:case="http://xbrl.org/2008/conformance" 
  xmlns:s="http://xbrl.org/conformance/2008/specifications"
  >
  
<xsl:variable name="specifications" select="document('specifications.xml')/*/*"/>
<!-- BODGE for dev only-->
<xsl:variable name="specRoot">../../../../xii-rendering/inlineXBRL/spec</xsl:variable>
  
<xsl:template match="/">
<xsl:apply-templates select="case:testcase"/>
</xsl:template>  
  
<xsl:template match="case:testcase">
  <html>
    <head>
      <title>
        <xsl:value-of select="case:number" />
        :
        <xsl:value-of select="case:name" />
      </title>
    </head>
    <body>
        
      <h1>
        <xsl:value-of select="case:name" />
      </h1>
      
      <p>
        Created by: 
        <a href="mailto: {case:creator/case:email}">
          <xsl:value-of select="case:creator/case:name" />
        </a>
      </p>

      <xsl:if test="count(case:reference) &gt; 0">
        <h3>References</h3>
        <ul>
          <xsl:apply-templates select="case:reference"/>
        </ul>
      </xsl:if>
      
      <h2>
        Test Details
      </h2>
      
      <table border="solid" cellpadding="5">
        <thead>
          <tr>
            <th>Number</th>
            <th>Description</th>
            <th>Data Inputs</th>
          </tr>
        </thead>
        <tbody>
          <xsl:apply-templates select="case:variation" >
            <xsl:with-param name="number">
		    			<xsl:value-of select="case:number"/>
				    </xsl:with-param>
          </xsl:apply-templates>
        </tbody>
      </table>
    </body>
  </html>
</xsl:template>
    
<xsl:template match="case:variation">
  <xsl:param name="number"/>
  <tr>
    <td align="center">
      <xsl:value-of select="$number"/>
    </td>
    <td>
      <xsl:value-of select="case:description"/>
    </td>
    <td>
      <xsl:apply-templates select="case:data"/>
    </td>
  </tr>
</xsl:template>

<xsl:template match="case:data | case:result">
  <ul>
    <xsl:if test="@expected" >
      <xsl:apply-templates select="@expected" />
    </xsl:if>
    <xsl:apply-templates select="*"/>
  </ul>
</xsl:template>

<xsl:template match="case:instance">
  <li>
    Instance
    <a href="{text()}">
      <xsl:value-of select="text()"/>
    </a>
    <xsl:apply-templates select="@readMeFirst"/>
  </li>
</xsl:template>

<xsl:template match="case:schema">
  <li>
    Schema
    <a href="{text()}">
      <xsl:value-of select="text()"/>
    </a>
    <xsl:apply-templates select="@readMeFirst"/>
  </li>
</xsl:template>

<xsl:template match="case:linkbase">
  <li>
    Linkbase
    <a href="{text()}">
      <xsl:value-of select="text()"/>
    </a>
    <xsl:apply-templates select="@readMeFirst"/>
  </li>
</xsl:template>

<xsl:template match="case:parameter">
  <li>
    <table align="top">
    <tr><th colspan="2" align="left">Parameter</th></tr>
    <tr><td>Name</td><td><xsl:value-of select="@name"/></td></tr>
    <tr><td>Type</td><td><xsl:value-of select="@datatype"/></td></tr>
    <tr><td>Value</td><td><xsl:value-of select="@value"/></td></tr>
    </table>
  </li>
</xsl:template>

<xsl:template match="case:assertionTests">
  <li>
    <table align="top">
    <tr><th colspan="2" align="left">Assertion</th></tr>
    <tr><td colspan="2" align="left">ID: <xsl:value-of select="@assertionID"/></td></tr>
    <tr><td>Count satisfied</td><td><xsl:value-of select="@countSatisfied"/></td></tr>
    <tr><td>Count not satisfied</td><td><xsl:value-of select="@countNotSatisfied"/></td></tr>
    </table>
  </li>
</xsl:template>

<xsl:template match="case:error">
  <li>
      Error code: <xsl:value-of select="./text()"/>
  </li>
</xsl:template>

<xsl:template match="@readMeFirst"/>

<xsl:template match="case:reference">
  <li>
    <xsl:variable name="reference" select="@specification" />
    <xsl:variable name="url" select="concat($specRoot,'/',$specifications[@id=$reference]/@href)"/>
    <a href="{$url}#{@id}">
      <xsl:value-of select="@specification" />:
      <xsl:value-of select="@id" />
    </a>
  </li>
</xsl:template>


</xsl:stylesheet>

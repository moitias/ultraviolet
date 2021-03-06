﻿#version 130

uniform mat4 World;
uniform mat4 View;
uniform mat4 Projection;
uniform vec4 DiffuseColor;

attribute vec4 uv_Position0;

varying vec4 vColor;

void main()
{
	gl_Position = Projection * View * World * uv_Position0;
	vColor      = DiffuseColor;
}
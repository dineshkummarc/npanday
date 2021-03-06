package npanday.plugin.compile;

/*
 * Licensed to the Apache Software Foundation (ASF) under one
 * or more contributor license agreements.  See the NOTICE file
 * distributed with this work for additional information
 * regarding copyright ownership.  The ASF licenses this file
 * to you under the Apache License, Version 2.0 (the
 * "License"); you may not use this file except in compliance
 * with the License.  You may obtain a copy of the License at
 *
 *   http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing,
 * software distributed under the License is distributed on an
 * "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY
 * KIND, either express or implied.  See the License for the
 * specific language governing permissions and limitations
 * under the License.
 */

import npanday.InitializationException;
import npanday.LocalRepositoryUtil;
import npanday.assembler.AssemblerContext;
import npanday.resolver.NPandayDependencyResolution;
import org.apache.maven.artifact.resolver.ArtifactResolutionException;
import org.apache.maven.plugin.AbstractMojo;
import org.apache.maven.plugin.MojoExecutionException;
import org.apache.maven.project.MavenProject;

import java.io.File;


/**
 * This class initializes and validates the setup.
 *
 * @author Shane Isbell
 * @goal initialize
 * @phase compile
 * @description Initializes and validates the setup.
 *
 * @require
 */
public class ComponentInitializerMojo
    extends AbstractMojo
{

    /**
     * The maven project.
     *
     * @parameter expression="${project}"
     * @required
     */
    private MavenProject project;

    /**
     * @parameter expression="${settings.localRepository}"
     * @readonly
     */
    private File localRepository;

    /**
     * @component
     */
    private AssemblerContext assemblerContext;

    /**
     * @component
     */
    private NPandayDependencyResolution dependencyResolution;

    public void execute()
        throws MojoExecutionException
    {
        // TODO: sadly we must resolve dependencies here because of 'org.apache.maven.plugins:maven-remote-resources-plugin:1.2.1:process' running later
        try
        {
            dependencyResolution.require( project, LocalRepositoryUtil.create( localRepository ), "test" );
        }
        catch ( ArtifactResolutionException e )
        {
            throw new MojoExecutionException(
                "NPANDAY-901-003: Could not satisfy required dependencies for scope " + "test", e
            );
        }

        try
        {
            assemblerContext.init( project );
        }
        catch ( InitializationException e )
        {
            throw new MojoExecutionException( "NPANDAY-901-002: Failed to initialize the assembler context", e );
        }

        long endTime = System.currentTimeMillis();
    }
}

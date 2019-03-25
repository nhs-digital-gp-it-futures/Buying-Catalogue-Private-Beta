const SharePointProvider = require('./SharePointProvider')
const stream = require('stream')

class FakeFileStoreAPI {
  constructor () {
    this.folders = {}

    this.hasInitialised = false
    this.initialiseTestData().then(() => {
      this.hasInitialised = true // So you can check if the API has finished initialising
    })
  }

  /**
    Regression Test Data
   */

  async initialiseTestData () {
    /**
     * Regression Test Data
     *
     * Since Fakepoint is intended to serve as a tool for use in regression test suites,
     * we are populating it with a little bit of test data for standards compliance.
     * This means that standards can have a traceability matrix present.
     *
     * Id                                     StandardId  Status  Name
     * 23446cf9-d461-4907-b099-41f35b65de62	  S32	        -1	    Interoperability Standard
     * 2c8c9cc8-141b-4d06-872a-8d5e5efb3641	  S47	        0	      IM1 - Interface Mechanism
     * 3a7735f2-759d-4f49-bca0-0828f32cf86c	  S30	        2	      Information Governance
     * 3d10430f-1748-44ad-8df3-5d5d11544f75	  S49	        6	      Management Information (MI) Reporting
     * 522e58a7-6f14-4c28-9340-193482915401	  S21	        0	      Citizen Access
     * 64c5d608-772f-4bc2-905b-4cee5d0b6f91	  S27	        0	      Data Standards
     * 6cf61bc3-9714-4902-953f-76a238d5ffd5	  S4	        4	      View Record - Citizen - Standard
     * 719722d0-2354-437e-acdc-4625989bbca8	  S69	        0	      Testing
     * 7b62b29a-62a7-4b4a-bcc8-dfa65fb7e35c	  S65	        1	      Service Management
     * 99619bdd-6452-4850-9244-a4ce9bec70ca	  S63	        0	      Non-Functional Questions
     * 9e7780e9-7263-4ee4-a722-b3e1aaff4476	  S31	        0	      Commercial Standard
     * a5774787-3ab3-48ff-9fa2-6db06be1b926	  S25	        0	      Clinical Safety
     * cd07ffcf-822b-460a-9f94-be42ad9b995b	  S26	        0	      Data Migration
     * cdfdebda-edde-4af4-aaf4-9d59fca7cdaa	  S29	        5	      Hosting & Infrastructure
     * e70fd085-ec42-4901-a7ba-a0d6f61fbbe1	  S24	        0	      Business Continuity & Disaster Recovery
     * f49f91de-64fc-4cca-88bf-3238fb1de69b	  S28	        3	      Training
     */

    const claimsRequireingFiles = [
      'cd07ffcf-822b-460a-9f94-be42ad9b995b', // Data Migration
      '9e7780e9-7263-4ee4-a722-b3e1aaff4476', // Commercial Standard
      'a5774787-3ab3-48ff-9fa2-6db06be1b926', // Clinical Safety
      '719722d0-2354-437e-acdc-4625989bbca8', // Testing
      '23446cf9-d461-4907-b099-41f35b65de62', // Interoperability Standard
      'e70fd085-ec42-4901-a7ba-a0d6f61fbbe1', // Business Continuity & Disaster Recovery
      '7b62b29a-62a7-4b4a-bcc8-dfa65fb7e35c', // Service Management
      'f49f91de-64fc-4cca-88bf-3238fb1de69b', // Training
      '3a7735f2-759d-4f49-bca0-0828f32cf86c' // Information Governance
    ]

    // Claims for solution ID 9A74133F-A437-4B2C-A57C-0F8B4EDDEA31, used to test
    // Capability ID C1, claim ID 7C0D087F-445A-4D13-8B66-7749F004CFEB
    // Standards ID S1, claim ID A36D09B6-CEB3-4C52-B8B4-58AE35E91F12
    // the Standards Compliance screens in their unsubmittable form (that is,
    // the solution has been submitted for assessment but has not yet completed it)
    const apptMgmtGpStdClaimID = 'A36D09B6-CEB3-4C52-B8B4-58AE35E91F12'

    const fakeBuffer = Buffer.from('Content of the test "Dummy traceabilityMatrix.xlsx" file', 'utf8')
    const fakeName = 'Dummy TraceabilityMatrix.xlsx'

    await Promise.all(
      claimsRequireingFiles.map(async (claimId) => this.addTestItem(claimId, fakeBuffer, fakeName))
    )

    await this.addTestItem(apptMgmtGpStdClaimID, fakeBuffer, fakeName)
  }

  addTestItem (claimID, buffer, filename) {
    if (!this.folders[claimID]) {
      this.initialiseClaim(claimID)
    }

    const guid = require('node-uuid-generator').generate()

    const newItem = {
      name: filename,
      isFolder: false,
      length: buffer.length,
      url: `FakeSharePoint.${guid}`,
      timeLastModified: (new Date()).toISOString(),
      blobId: `FakeSharePoint.${guid}`,
      buffer: buffer
    }

    this.folders[claimID].items.push(newItem)
  }

  initialiseClaim (claimID) {
    this.folders[claimID] = {
      pageIndex: 0,
      totalPages: 0,
      pageSize: 0,
      items: [],
      hasPreviousPage: false,
      hasNextPage: false
    }
  }

  async enumerateAllClaims () {
    const enumeration = {
      items: Object.keys(this.folders).map((claimId) => {
        return {
          claimId: claimId,
          blobInfos: this.folders[claimId].items
        }
      })
    }
    return enumeration
  }

  async addItemToClaim (claimID, readStream, filename, options) {
    if (!this.folders[claimID]) {
      this.initialiseClaim(claimID)
    }

    const guid = require('node-uuid-generator').generate()
    const buffer = await this.readStream(readStream)

    const newItem = {
      name: filename,
      isFolder: false,
      length: buffer.length,
      url: `FakeSharePoint.${guid}`,
      timeLastModified: (new Date()).toISOString(),
      blobId: `FakeSharePoint.${guid}`,
      buffer: buffer
    }

    this.folders[claimID].items.push(newItem)
  }

  async enumerateFolder (claimID) {
    if (!this.folders[claimID]) {
      this.initialiseClaim(claimID)
    }
    return this.folders[claimID]
  }

  async readStream (readStream) {
    return new Promise((resolve, reject) => {
      var buffers = []
      readStream.on('data', (data) => buffers.push(data))
      readStream.on('end', () => resolve(Buffer.concat(buffers)))
    })
  }

  async downloadFile (claimID, opts) {
    if (!this.folders[claimID]) {
      return Promise.reject(new Error('Claim Not Found'))
    }

    const item = this.folders[claimID].items.find((item) => item.blobId === opts.uniqueId)

    if (!item) {
      return Promise.reject(new Error('File Not Found'))
    }

    const bufferStream = new stream.PassThrough()
    bufferStream.end(item.buffer)
    return { body: bufferStream }
  }
}

class FakeStandardsApplicableEvidenceBlobStoreApi extends FakeFileStoreAPI {
  async apiStandardsApplicableEvidenceBlobStoreEnumerateFolderByClaimIdGet (claimID, options) {
    return this.enumerateFolder(claimID)
  }

  async apiStandardsApplicableEvidenceBlobStoreAddEvidenceForClaimPost (claimID, readStream, filename, options) {
    return this.addItemToClaim(claimID, readStream, filename, options)
  }

  async apiStandardsApplicableEvidenceBlobStoreDownloadByClaimIdPost (claimID, opts) {
    return this.downloadFile(claimID, opts)
  }

  async apiStandardsApplicableEvidenceBlobStoreEnumerateClaimFolderTreeBySolutionIdGet (solutionId, opts) {
    return this.enumerateAllClaims()
  }
}

class FakeCapabilitiesImplementedEvidenceBlobStoreApi extends FakeFileStoreAPI {
  async apiCapabilitiesImplementedEvidenceBlobStoreEnumerateFolderByClaimIdGet (claimID, options) {
    return this.enumerateFolder(claimID)
  }

  async apiCapabilitiesImplementedEvidenceBlobStoreAddEvidenceForClaimPost (claimID, readStream, filename, options) {
    return this.addItemToClaim(claimID, readStream, filename, options)
  }

  async apiCapabilitiesImplementedEvidenceBlobStoreDownloadByClaimIdPost (claimID, opts) {
    return this.downloadFile(claimID, opts)
  }

  async apiCapabilitiesImplementedEvidenceBlobStoreEnumerateClaimFolderTreeBySolutionIdGet (solutionId) {
    // Enumerates all claims in FakePoint since there is no way to which claims are associated to a solution
    // If the user of this method is performing adequate checks of which Capabilities are actually being
    // claimed by a solution, then those not applicable will be filtered out.
    return this.enumerateAllClaims()
  }
}

const FakeCatalogueAPI = {
  StandardsApplicableEvidenceBlobStoreApi: FakeStandardsApplicableEvidenceBlobStoreApi,
  CapabilitiesImplementedEvidenceBlobStoreApi: FakeCapabilitiesImplementedEvidenceBlobStoreApi
}

class FakeSharePointProvider extends SharePointProvider {
  constructor () {
    console.info('Mock Sharepoint provider active')
    super(FakeCatalogueAPI)
  }

  // This overrides the SharePointProvider implementation of DownloadCapEvidence.
  // When the problems with Swashbuckle and the autogenerated Swagger schema and
  // Code are resolved, this method would not need to exist, and it can be faked
  // in the same way as the other methods
  async downloadCapEvidence (claimID, uniqueId) {
    return this.capBlobStoreApi.apiCapabilitiesImplementedEvidenceBlobStoreDownloadByClaimIdPost(claimID, { uniqueId })
  }

  // This overrides the SharePointProvider implementation of DownloadCapEvidence.
  // When the problems with Swashbuckle and the autogenerated Swagger schema and
  // Code are resolved, this method would not need to exist, and it can be faked
  // in the same way as the other methods
  async downloadStdEvidence (claimID, uniqueId) {
    return this.stdBlobStoreApi.apiStandardsApplicableEvidenceBlobStoreDownloadByClaimIdPost(claimID, { uniqueId })
  }

  removeFolder () {}
}

module.exports = FakeSharePointProvider

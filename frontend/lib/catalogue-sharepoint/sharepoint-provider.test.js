/* eslint-env jest */

const { SharePointProvider } = require('./index')

function MockCapBlobStoreApi () {
  this.getCapEvidence = jest.fn()
}

function MockStdBlobStoreApi () {
  this.getStdEvidence = jest.fn()
}

let subject

beforeAll(() => {
  subject = new SharePointProvider({
    CapabilitiesImplementedEvidenceBlobStoreApi: MockCapBlobStoreApi,
    StandardsApplicableEvidenceBlobStoreApi: MockStdBlobStoreApi
  })
  subject.uuidGenerator = { generate: () => { return 'UUID-1234-ABCD' } }
})

describe('deleteFile', () => {
  const fn = 'test_file.txt'

  beforeEach(() => {
    subject.createStoragePath = jest.fn()
    subject.unlinkFile = jest.fn()
    subject.createFileStoragePath = jest.fn()

    subject.createFileStoragePath.mockImplementation(() => {
      return fn
    })
  })

  it('should try and create the storage paths with filename.', () => {
    subject.deleteFile(fn)

    expect(subject.createFileStoragePath).toBeCalledWith(fn)
  })

  it('should try and unlink the a file at the specified storage path', () => {
    subject.deleteFile(fn)
    const expectedPath = fn
    expect(subject.unlinkFile).toBeCalledWith(expectedPath, expect.any(Function))
  })

  it('Should return a promise that resolves if file successfully unlinks', async () => {
    subject.unlinkFile.mockImplementation((fp, cb) => {
      cb(/* No Errors */)
    })
    await expect(subject.deleteFile(fn)).resolves.toBe(undefined)
  })

  it('Should return a promise that rejects if file failes to unlinks', async () => {
    subject.unlinkFile.mockImplementation((fp, cb) => {
      cb('test_error_message')
    })
    await expect(subject.deleteFile(fn)).rejects.toBe('test_error_message')
  })
})

describe('createFileReadStream', () => {
  const fn = 'file'
  const claimFilePath = fn

  beforeEach(() => {
    subject.createReadStream = jest.fn()
    subject.createFileStoragePath = jest.fn()

    subject.createFileStoragePath.mockImplementation(() => {
      return claimFilePath
    })
  })

  it('Should try and create storage path', () => {
    subject.createFileReadStream(fn)
    expect(subject.createFileStoragePath).toBeCalledWith(fn)
  })

  it('Should try and create a fileReadStream', () => {
    subject.createFileReadStream(fn)
    expect(subject.createReadStream).toBeCalledWith(claimFilePath)
  })

  it('Should throw if creation of storage path throws', () => {
    subject.createFileStoragePath.mockImplementation(() => {
      throw new Error()
    })
    let throws
    try {
      subject.createFileReadStream(fn)
    } catch (err) {
      throws = true
    }
    expect(throws).toBeTruthy()
  })

  it('Should throw if creation of file read stream throws', () => {
    subject.createFileStoragePath.mockImplementation(() => {
      throw new Error()
    })
    let throws
    try {
      subject.createFileReadStream(fn)
    } catch (err) {
      throws = true
    }
    expect(throws).toBeTruthy()
  })
})

describe('saveBuffer', () => {
  const fn = 'test_file.txt'
  const buffer = Buffer.from('test string', 'utf-8')

  beforeEach(() => {
    subject.createStoragePath = jest.fn()
    subject.writeFile = jest.fn()

    subject.createFileStoragePath.mockImplementation(() => {
      return fn
    })
  })

  it('Should try and create storage path before writing to it.', () => {
    subject.saveBuffer(buffer, fn)
    expect(subject.createFileStoragePath).toBeCalledWith(fn)
  })

  it('should try and write a file providing storage path, buffer, and a callback', () => {
    subject.saveBuffer(buffer, fn)
    expect(subject.writeFile).toBeCalledWith(fn, buffer, expect.any(Function))
  })

  it('Should return a resolvable promise if file writing is successful', async () => {
    subject.writeFile.mockImplementation((sp, bf, cb) => {
      cb(/*No Error*/)
    })

    await expect(subject.saveBuffer(buffer, fn)).resolves.toBe(undefined)
  })

  it('Should return a rejecting promise if file writing is successful', async () => {
    subject.writeFile.mockImplementation((sp, bf, cb) => {
      cb('test_error_message')
    })

    await expect(subject.saveBuffer(buffer, fn)).rejects.toBe('test_error_message')
  })
})

describe('uploadEvidence', () => {
  const mm = jest.fn()
  const cm = '1234'
  const bf = Buffer.from('test_file')
  const fn = 'test_file.txt'
  const sb = '/test_sub/'
  const rs = 'test_read_stream'
  const uuidfn = `${fn}-UUID-1234-ABCD`

  beforeEach(() => {
    subject.saveBuffer = jest.fn()
    subject.deleteFile = jest.fn()
    subject.createFileReadStream = jest.fn()
  })

  it('Should save buffer to file forwarding it on', async () => {
    await subject.uploadEvidence(mm, cm, bf, fn, sb)
    expect(subject.saveBuffer).toBeCalledWith(bf, uuidfn)
  })

  it('Should create a file read stream', async () => {
    await subject.uploadEvidence(mm, cm, bf, fn, sb)
    expect(subject.createFileReadStream).toBeCalledWith(uuidfn)
  })

  it('Should call the provided method with params needed for posting file data', async () => {
    const mockMethod = jest.fn()
    mockMethod.mockImplementation((cm, rs, fn, op) => {
      return fn
    })
    subject.createFileReadStream.mockImplementation(() => {
      return rs
    })
    const options = { subFolder: sb }

    await subject.uploadEvidence(mockMethod, cm, bf, fn, sb)
    expect(mockMethod).toBeCalledWith(cm, rs, fn, options)
  })

  it('should call the file delete method after it has uploaded a response', async () => {
    await subject.uploadEvidence(mm, cm, bf, fn, sb)
    expect(subject.deleteFile).toBeCalledWith(uuidfn)
  })

  it('should return a rejecting promise with error message if saving buffer fails', async () => {
    subject.saveBuffer.mockImplementation(() => {
      throw 'error'
    })
    await expect(subject.uploadEvidence(mm, cm, bf, fn, sb)).rejects.toBe('error')
  })

  it('should return a rejecting promise with error message if creating a readStream fails', async () => {
    subject.createFileReadStream.mockImplementation(() => {
      throw 'error'
    })
    await expect(subject.uploadEvidence(mm, cm, bf, fn, sb)).rejects.toBe('error')
  })

  it('should return a rejecting promise with error message if provided method fails', async () => {
    const mockMethod = jest.fn()
    mockMethod.mockImplementation(() => {
      throw 'error'
    })
    await expect(subject.uploadEvidence(mockMethod, cm, bf, fn, sb)).rejects.toBe('error')
  })

  it('should return a rejecting Promise with error message if deleting file fails', async () => {
    subject.deleteFile.mockImplementation(() => {
      throw 'error'
    })
    await expect(subject.uploadEvidence(mm, cm, bf, fn, sb)).rejects.toBe('error')
  })

  it('Should return a resolving promise with the result from the provided method', async () => {
    const mockMethod = jest.fn()
    mockMethod.mockImplementation(() => {
      return fn
    })
    await expect(subject.uploadEvidence(mockMethod, cm, bf, fn, sb)).resolves.toBe(fn)
  })
})

describe('uploadStdEvidence', () => {
  const cm = 'claim'
  const bf = Buffer.from('test buffer')
  const fn = 'test_file.txt'
  const sb = '/sub_folder/'

  beforeEach(() => {
    subject.stdBlobStoreApi.apiStandardsApplicableEvidenceBlobStoreAddEvidenceForClaimPost = jest.fn()
    subject.uploadEvidence = jest.fn()
  })

  it('Should provide the standards upload method to the upload evidence method', async () => {
    await subject.uploadStdEvidence(cm, bf, fn, sb)
    expect(subject.uploadEvidence).toBeCalledWith(expect.any(Function), cm, bf, fn, sb)

    // stringified for comparison
    const firstParam = JSON.stringify(subject.uploadEvidence.mock.calls[0][0])
    const expectedParam = JSON.stringify(
      subject.stdBlobStoreApi.apiStandardsApplicableEvidenceBlobStoreAddEvidenceForClaimPost.bind(subject.stdBlobStoreApi)
    )
    // First param
    expect(firstParam).toEqual(expectedParam)
  })

  it('Should return a rejecting promise if invoked methods fail', async () => {
    subject.uploadEvidence.mockImplementation(() => {
      return Promise.reject('error')
    })

    await expect(subject.uploadStdEvidence(cm, bf, fn, sb)).rejects.toBe('error')
  })

  it('Should return a resolving promise if no internal invokations fail', async () => {
    subject.uploadEvidence.mockImplementation(() => {
      return Promise.resolve()
    })

    await expect(subject.uploadStdEvidence(cm, bf, fn, sb)).resolves.toBe(undefined)
  })
})

describe('getCapEvidence', () => {
  it('Should recieve evidence listings for a capability claim with evidence against it', async () => {
    subject.capBlobStoreApi.getCapEvidence.mockReturnValue(
      require('./sharepoint-cap.test-data.json')
    )

    const res = await subject.capBlobStoreApi.getCapEvidence()
    expect(res.items).toHaveLength(6)
  })

  it('Evidence items should all have a name, an isFolderFlag, a URL, and a timestamp', async () => {
    subject.capBlobStoreApi.getCapEvidence.mockReturnValue(
      require('./sharepoint-cap.test-data.json')
    )

    const res = await subject.capBlobStoreApi.getCapEvidence()
    expect(res.items.every((item) =>
      item.name && item.isFolder && item.url && item.timeLastModified
    ))
  })
})

describe('uploadCapEvidence', () => {
  const cm = 'claim'
  const bf = Buffer.from('test buffer')
  const fn = 'test_file.txt'
  const sb = '/sub_folder/'

  beforeEach(() => {
    subject.capBlobStoreApi.apiCapabilitiesImplementedEvidenceBlobStoreAddEvidenceForClaimPost = jest.fn()
    subject.uploadEvidence = jest.fn()
  })

  it('Should provide the capability upload method to the upload evidence method', async () => {
    await subject.uploadCapEvidence(cm, bf, fn, sb)
    expect(subject.uploadEvidence).toBeCalledWith(expect.any(Function), cm, bf, fn, sb)

    // stringified for comparison
    const firstParam = JSON.stringify(subject.uploadEvidence.mock.calls[0][0])
    const expectedParam = JSON.stringify(
      subject.capBlobStoreApi.apiCapabilitiesImplementedEvidenceBlobStoreAddEvidenceForClaimPost.bind(subject.capBlobStoreApi)
    )
    // First param
    expect(firstParam).toEqual(expectedParam)
  })

  it('Should return a rejecting promise if invoked methods fail', async () => {
    subject.uploadEvidence.mockImplementation(() => {
      return Promise.reject('error')
    })

    await expect(subject.uploadCapEvidence(cm, bf, fn, sb)).rejects.toBe('error')
  })

  it('Should return a resolving promise if no internal invokations fail', async () => {
    subject.uploadEvidence.mockImplementation(() => {
      return Promise.resolve()
    })

    await expect(subject.uploadCapEvidence(cm, bf, fn, sb)).resolves.toBe(undefined)
  })
})
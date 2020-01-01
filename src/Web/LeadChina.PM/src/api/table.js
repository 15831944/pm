import request from '@/utils/request'

export function getList(params) {
  return request.service({
    url: '/table/list',
    method: 'get',
    params
  })
}

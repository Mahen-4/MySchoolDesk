import React, {ChangeEvent } from "react"
import Axios, { AxiosError } from "axios";
function AddStudent() {

    const [selectedFile, setSelectedFile] = React.useState<File | null>(null);
    const [fileDownload, setFileDownload] = React.useState<string | null>(null)

    const handleClick = async() => {
        if (!selectedFile) return;

        try {
            console.log("truy")
            const formData = new FormData();
            formData.append('file', selectedFile) 
            await Axios.post("http://localhost:5271/ManageStudent/upload", formData,{headers:{'Content-Type': 'multipart/form-data'}, responseType: "blob"})
            .then(res => {
              console.log(res)
              const blob = new Blob([res.data], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' })
              const url = window.URL.createObjectURL(blob)
              setFileDownload(url)
            })

        } catch (error: any  ) {
            if(error.response.status == 401){
              console.log("one field values do not respect the law of the app")
            }
            else{
              console.log("Intern Error OR File Type not allowed ")
            }
        }
    }
    const handleFileChange = (event: ChangeEvent<HTMLInputElement>) => {
        if (event.target.files && event.target.files.length > 0) {
            setSelectedFile(event.target.files[0]);
          }
      };
    return (
      <>
        <input type="file" onChange={handleFileChange}/>
        <button onClick={handleClick}>
            Create
        </button>
        {fileDownload && (
          <a href={fileDownload} download="Students-Data">Downloaded File</a>
        )}
      </>
    )
  }
  
  export default AddStudent